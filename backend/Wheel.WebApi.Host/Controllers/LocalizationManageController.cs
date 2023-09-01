using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wheel.Core.Http;
using Wheel.Services.LocalizationManage;
using Wheel.Services.LocalizationManage.Dtos;
using Wheel.Utilities;

namespace Wheel.Controllers
{
    /// <summary>
    /// 多语言管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizationManageController : WheelControllerBase
    {
        private readonly ILocalizationManageAppService _localizationManageAppService;

        public LocalizationManageController(ILocalizationManageAppService localizationManageAppService)
        {
            _localizationManageAppService = localizationManageAppService;
        }

        /// <summary>
        /// 获取地区多语言详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Culture/{id}")]
        public async Task<R<LocalizationCultureDto>> GetCulture(int id)
        {
            var value = await _localizationManageAppService.GetLocalizationCultureAsync(id);
            return new R<LocalizationCultureDto>(value);
        }
        /// <summary>
        /// 创建地区多语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Culture")]
        public async Task<R<LocalizationCultureDto>> CreateCulture(CreateLocalizationCultureDto input)
        {
            var culture = await _localizationManageAppService.CreateLocalizationCultureAsync(input);
            return new R<LocalizationCultureDto>(culture);
        }
        /// <summary>
        /// 删除地区多语言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Culture/{id}")]
        public async Task<R> DeleteCulture(int id)
        {
            await _localizationManageAppService.DeleteLocalizationCultureAsync(id);
            return new R();
        }
        /// <summary>
        /// 分页获取地区多语言列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("Culture")]
        public async Task<Page<LocalizationCultureDto>> GetCulturePageList([FromQuery]PageRequest input)
        {
            return await _localizationManageAppService.GetLocalizationCulturePageListAsync(input);
        }
        /// <summary>
        /// 创建多语言资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Resource")]
        public async Task<R<LocalizationResourceDto>> CreateResource(CreateLocalizationResourceDto input)
        {
            var resource = await _localizationManageAppService.CreateLocalizationResourceAsync(input);
            return new R<LocalizationResourceDto>(resource);
        }
        /// <summary>
        /// 修改多语言资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("Resource")]
        public async Task<R> UpdateResource(UpdateLocalizationResourceDto input)
        {
            await _localizationManageAppService.UpdateLocalizationResourceAsync(input);
            return new R();
        }
        /// <summary>
        /// 删除多语言资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Resource/{id}")]
        public async Task<R> DeleteResource(int id)
        {
            await _localizationManageAppService.DeleteLocalizationResourceAsync(id);
            return new R();
        }
    }
}
