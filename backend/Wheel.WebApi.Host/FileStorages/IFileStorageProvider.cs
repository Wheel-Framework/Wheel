using Minio;
using Wheel.DependencyInjection;

namespace Wheel.FileStorages
{
    public interface IFileStorageProvider : ITransientDependency
    {
        string Name { get; }

        Task<UploadFileResult> Upload(UploadFileArgs uploadFileArgs, CancellationToken cancellationToken = default);
        Task<DownFileResult> Download(DownloadFileArgs downloadFileArgs, CancellationToken cancellationToken = default);

        Task<object> GetClient();

        void ConfigureClient<T>(Action<T> configure);

    }
}
