namespace Wheel.Administrator.Services.FileStorageManage.Dtos
{
    public class DownloadFileResonse
    {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
