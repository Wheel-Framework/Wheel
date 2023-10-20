using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.Security.AccessControl;
using Wheel.Migrations;
using Wheel.Settings;

namespace Wheel.FileStorages.Providers
{
    public class MinioFileStorageProvider : IFileStorageProvider
    {
        private readonly ISettingProvider _settingProvider;
        private readonly ILogger<MinioFileStorageProvider> _logger;

        public MinioFileStorageProvider(ISettingProvider settingProvider, ILogger<MinioFileStorageProvider> logger)
        {
            _settingProvider = settingProvider;
            _logger = logger;
        }

        public string Name => "Minio";
        internal Action<IMinioClient>? Configure { get; private set; }
        public async Task<UploadFileResult> Upload(UploadFileArgs uploadFileArgs, CancellationToken cancellationToken = default)
        {
            var client = await GetMinioClient();
            try
            {
                // Make a bucket on the server, if not already present.
                var beArgs = new BucketExistsArgs()
                    .WithBucket(uploadFileArgs.BucketName);
                bool found = await client.BucketExistsAsync(beArgs, cancellationToken).ConfigureAwait(false);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(uploadFileArgs.BucketName);
                    await client.MakeBucketAsync(mbArgs, cancellationToken).ConfigureAwait(false);
                }
                // Upload a file to bucket.
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(uploadFileArgs.BucketName)
                    .WithObject(uploadFileArgs.FileName)
                    .WithStreamData(uploadFileArgs.FileStream)
                    .WithObjectSize(uploadFileArgs.FileStream.Length)
                    .WithContentType(uploadFileArgs.ContentType);
                await client.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);
                var path = BuildPath(uploadFileArgs.BucketName, uploadFileArgs.FileName);
                _logger.LogInformation("Successfully Uploaded " + path);
                return new UploadFileResult { FilePath = path, Success = true };
            }
            catch (MinioException e)
            {
                _logger.LogError("File Upload Error: {0}", e.Message);
                return new UploadFileResult { Success = false };
            }
        }
        public async Task<DownFileResult> Download(DownloadFileArgs downloadFileArgs, CancellationToken cancellationToken = default)
        {
            var client = await GetMinioClient();
            try
            {
                var stream = new MemoryStream();
                var args = downloadFileArgs.Path.Split("/");
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(args[0])
                    .WithObject(downloadFileArgs.Path.RemovePreFix($"{args[0]}/"))
                    .WithCallbackStream(fs => fs.CopyTo(stream))
                    ;
                var response = await client.GetObjectAsync(getObjectArgs, cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("Successfully Download " + downloadFileArgs.Path);
                stream.Position = 0;
                return new DownFileResult { Stream = stream, Success = true, FileName = response.ObjectName, ContentType = response.ContentType };
            }
            catch (MinioException e)
            {
                _logger.LogError("File Download Error: {0}", e.Message);
                return new DownFileResult { Success = false };
            }
        }

        public async Task<object> GetClient()
        {
            return await GetMinioClient();
        }

        public void ConfigureClient<T>(Action<T> configure)
        {
            if (typeof(T) == typeof(IMinioClient))
                Configure = configure as Action<IMinioClient>;
            else
                throw new Exception("MinioFileProvider ConfigureClient Only Can Configure Type With IMinioClient");
        }

        private async Task<IMinioClient> GetMinioClient()
        {
            var minioSetting = await GetSettings();
            var client = new MinioClient()
                .WithHttpClient(new HttpClient())
                .WithEndpoint(minioSetting["Endpoint"])
                .WithCredentials(minioSetting["AccessKey"], minioSetting["SecretKey"])
                .WithSessionToken(minioSetting["SessionToken"]);

            if (!string.IsNullOrWhiteSpace(minioSetting["Region"]))
            {
                client.WithRegion(minioSetting["Region"]);
            }

            if(Configure != null)
            {
                Configure.Invoke(client);
            }
            return client;
        }

        private async Task<Dictionary<string, string>> GetSettings()
        {
            var settings = await _settingProvider.GetGolbalSettings("FileProvider");

            return settings.Where(a => a.Key.StartsWith("Minio")).ToDictionary(a => a.Key.RemovePreFix("Minio."), a => a.Value);
        }
        private string BuildPath(string bucketName, string fileName)
        {
            return string.Join('/', bucketName, fileName);
        }
    }
}
