using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wheel.Administrator.Services.FileStorageManage.Dtos
{
    public class UploadFileDto
    {
        [FromQuery]
        public bool Cover { get; set; } = false;

        [FromQuery]
        public string? Provider { get; set; }

        [FromForm]
        public IFormFileCollection Files { get; set; }
    }
}
