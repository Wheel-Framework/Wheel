using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wheel.Core.Dto;
using Wheel.Services.FileStorageManage;
using Wheel.Services.FileStorageManage.Dtos;

namespace Wheel.Controllers
{
    /// <summary>
    /// 文件管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : WheelControllerBase
    {
        private readonly IFileStorageManageAppService _fileStorageManageAppService;

        public FileController(IFileStorageManageAppService fileStorageManageAppService)
        {
            _fileStorageManageAppService = fileStorageManageAppService;
        }
        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<Page<FileStorageDto>> GetFileStoragePageList([FromQuery] FileStoragePageRequest request)
        {
            return _fileStorageManageAppService.GetFileStoragePageList(request);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="uploadFileDto"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<R<List<FileStorageDto>>> UploadFiles(UploadFileDto uploadFileDto)
        {
            return _fileStorageManageAppService.UploadFiles(uploadFileDto);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadFile(long id)
        {
            var result = await _fileStorageManageAppService.DownloadFile(id);
            return File(result.Data.Stream, result.Data.ContentType, result.Data.FileName);
        }
    }
}
