using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wheel.Core.Http;
using Wheel.Services.LocalizationManage;
using Wheel.Services.LocalizationManage.Dtos;
using Wheel.Utilities;

namespace Wheel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizationManageController : WheelControllerBase
    {
        private readonly ILocalizationManageAppService _localizationManageAppService;

        public LocalizationManageController(ILocalizationManageAppService localizationManageAppService)
        {
            _localizationManageAppService = localizationManageAppService;
        }

        [HttpGet("Culture/{id}")]
        public async Task<R<LocalizationCultureDto>> GetCulture(int id)
        {
            var value = await _localizationManageAppService.GetLocalizationCultureAsync(id);
            return new R<LocalizationCultureDto>(value);
        }
        [HttpPost("Culture")]
        public async Task<R<LocalizationCultureDto>> CreateCulture(CreateLocalizationCultureDto input)
        {
            var culture = await _localizationManageAppService.CreateLocalizationCultureAsync(input);
            return new R<LocalizationCultureDto>(culture);
        }
        [HttpDelete("Culture/{id}")]
        public async Task<R> DeleteCulture(int id)
        {
            await _localizationManageAppService.DeleteLocalizationCultureAsync(id);
            return new R();
        }
        [HttpGet("Culture")]
        public async Task<Page<LocalizationCultureDto>> GetCulturePageList([FromQuery]PageRequest input)
        {
            return await _localizationManageAppService.GetLocalizationCulturePageListAsync(input);
        }
    }
}
