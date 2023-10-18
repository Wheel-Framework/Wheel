using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wheel.Core.Dto;
using Wheel.Enums;
using Wheel.Services.SupportData;
using Wheel.Services.SupportData.Dtos;

namespace Wheel.Controllers
{
    /// <summary>
    /// 基础支持数据
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SupportDataController : ControllerBase
    {
        private readonly ISupportDataAppService _supportDataAppService;
        public SupportDataController(ISupportDataAppService supportDataAppService)
        {
            _supportDataAppService = supportDataAppService;
        }
        /// <summary>
        /// 获取枚举键值对数据
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="nameSpace">枚举命名空间，默认Wheel.Enums</param>
        /// <returns></returns>
        [HttpGet("Enums/{type}")]
        public ValueTask<R<List<LabelValueDto>>> GetEnums(string type, string? nameSpace)
        {
            return _supportDataAppService.GetEnums(type, nameSpace);
        }

    }
}
