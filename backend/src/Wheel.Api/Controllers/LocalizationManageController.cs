using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Wheel.Core.Dto;
using Wheel.Services.LocalizationManage;
using Wheel.Services.LocalizationManage.Dtos;

namespace Wheel.Controllers
{
    /// <summary>
    /// 多语言管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizationManageController
        (ILocalizationManageAppService localizationManageAppService) : WheelControllerBase
    {
        /// <summary>
        /// 获取地区多语言详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Culture/{id}")]
        public async Task<R<LocalizationCultureDto>> GetCulture(int id)
        {
            return await localizationManageAppService.GetLocalizationCultureAsync(id);
        }
        /// <summary>
        /// 创建地区多语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Culture")]
        public async Task<R<LocalizationCultureDto>> CreateCulture(CreateLocalizationCultureDto input)
        {
            return await localizationManageAppService.CreateLocalizationCultureAsync(input);
        }
        /// <summary>
        /// 删除地区多语言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Culture/{id}")]
        public async Task<R> DeleteCulture(int id)
        {
            return await localizationManageAppService.DeleteLocalizationCultureAsync(id);
        }
        /// <summary>
        /// 分页获取地区多语言列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("Culture")]
        public async Task<Page<LocalizationCultureDto>> GetCulturePageList([FromQuery] PageRequest input)
        {
            return await localizationManageAppService.GetLocalizationCulturePageListAsync(input);
        }
        /// <summary>
        /// 创建多语言资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Resource")]
        public async Task<R<LocalizationResourceDto>> CreateResource(CreateLocalizationResourceDto input)
        {
            return await localizationManageAppService.CreateLocalizationResourceAsync(input);
        }
        /// <summary>
        /// 修改多语言资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("Resource")]
        public async Task<R> UpdateResource(UpdateLocalizationResourceDto input)
        {
            return await localizationManageAppService.UpdateLocalizationResourceAsync(input);
        }
        /// <summary>
        /// 删除多语言资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Resource/{id}")]
        public async Task<R> DeleteResource(int id)
        {
            return await localizationManageAppService.DeleteLocalizationResourceAsync(id);
        }
        /// <summary>
        /// 获取多语言资源列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("Resources")]
        [AllowAnonymous]
        public Task<R<Dictionary<string, string>>> GetResources()
        {
            var resources = L.GetAllStrings().ToDictionary(a => a.Name, a => a.Value);
            return Task.FromResult(new R<Dictionary<string, string>>(resources));
        }
    }
}
