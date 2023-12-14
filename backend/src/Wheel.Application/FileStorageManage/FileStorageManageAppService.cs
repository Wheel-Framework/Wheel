using Microsoft.Extensions.DependencyInjection;
using Wheel.Const;
using Wheel.Core.Dto;
using Wheel.Core.Exceptions;
using Wheel.Domain;
using Wheel.Domain.FileStorages;
using Wheel.Enums;
using Wheel.FileStorages;
using Wheel.Services.FileStorageManage.Dtos;
using Path = System.IO.Path;

namespace Wheel.Services.FileStorageManage
{
    public class FileStorageManageAppService
        (IBasicRepository<FileStorage, long> fileStorageRepository) : WheelServiceBase, IFileStorageManageAppService
    {
        public async Task<Page<FileStorageDto>> GetFileStoragePageList(FileStoragePageRequest request)
        {
            var (items, total) = await fileStorageRepository.GetPageListAsync(
                fileStorageRepository.BuildPredicate(
                    (!string.IsNullOrWhiteSpace(request.FileName), f => f.FileName.Contains(request.FileName!)),
                    (!string.IsNullOrWhiteSpace(request.ContentType), f => f.ContentType.Equals(request.ContentType)),
                    (!string.IsNullOrWhiteSpace(request.Path), f => f.Path.StartsWith(request.Path!)),
                    (!string.IsNullOrWhiteSpace(request.Provider), f => f.Provider.Equals(request.Provider)),
                    (request.FileStorageType.HasValue, f => f.FileStorageType.Equals(request.FileStorageType))
                    ),
                (request.PageIndex - 1) * request.PageSize,
                request.PageSize,
                request.OrderBy
                );

            return Page(Mapper.Map<List<FileStorageDto>>(items), total);
        }
        public async Task<R<List<FileStorageDto>>> UploadFiles(UploadFileDto uploadFileDto)
        {
            var files = uploadFileDto.Files;
            if (files.Count == 0)
                return new R<List<FileStorageDto>>(new());
            IFileStorageProvider? fileStorageProvider = null;
            var fileStorageProviders = ServiceProvider.GetServices<IFileStorageProvider>();
            if (string.IsNullOrWhiteSpace(uploadFileDto.Provider))
            {
                fileStorageProvider = fileStorageProviders.First();
            }
            else
            {
                fileStorageProvider = fileStorageProviders.First(a => a.Name == uploadFileDto.Provider);
            }
            var fileStorages = new List<FileStorage>();
            foreach (var file in files)
            {
                var fileName = uploadFileDto.Cover ? file.FileName : $"{Path.GetFileNameWithoutExtension(file.FileName)}-{SnowflakeIdGenerator.Create()}{Path.GetExtension(file.FileName)}";
                var fileStream = file.OpenReadStream();
                var fileStorageType = FileStorageTypeChecker.CheckFileType(file.ContentType);
                var uploadFileArgs = new UploadFileArgs
                {
                    BucketName = fileStorageType switch
                    {
                        FileStorageType.Image => "images",
                        FileStorageType.Video => "videos",
                        FileStorageType.Audio => "audios",
                        FileStorageType.Text => "texts",
                        _ => "files"
                    },
                    ContentType = file.ContentType,
                    FileName = fileName,
                    FileStream = fileStream
                };
                var uploadFileResult = await fileStorageProvider.Upload(uploadFileArgs);

                if (uploadFileResult.Success)
                {
                    var fileStorage = await fileStorageRepository.InsertAsync(new FileStorage
                    {
                        Id = SnowflakeIdGenerator.Create(),
                        ContentType = file.ContentType,
                        FileName = file.FileName,
                        FileStorageType = fileStorageType,
                        Path = uploadFileResult.FilePath,
                        Provider = fileStorageProvider.Name,
                        Size = fileStream.Length
                    });
                    await fileStorageRepository.SaveChangeAsync();
                    fileStorages.Add(fileStorage);
                }
            }
            return Success(Mapper.Map<List<FileStorageDto>>(fileStorages));
        }

        public async Task<R<DownloadFileResonse>> DownloadFile(long id)
        {
            var fileStorage = await fileStorageRepository.FindAsync(id);
            if (fileStorage == null)
            {
                throw new BusinessException(ErrorCode.FileNotExist, "FileNotExist")
                    .WithMessageDataData(id.ToString());
            }
            var fileStorageProvider = ServiceProvider.GetServices<IFileStorageProvider>().First(a => a.Name == fileStorage.Provider);

            var downloadResult = await fileStorageProvider.Download(new DownloadFileArgs { Path = fileStorage.Path });
            if (downloadResult.Success)
            {
                return Success(new DownloadFileResonse { ContentType = downloadResult.ContentType, FileName = downloadResult.FileName, Stream = downloadResult.Stream });
            }
            else
            {
                throw new BusinessException(ErrorCode.FileDownloadFail, "FileDownloadFail")
                    .WithMessageDataData(id.ToString());
            }
        }
    }
}
