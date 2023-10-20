namespace Wheel.FileStorages
{
    public class DownFileResult
    {
        public bool Success { get; set; }
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
