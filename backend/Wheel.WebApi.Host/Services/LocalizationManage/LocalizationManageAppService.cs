using Wheel.Core.Http;
using Wheel.Domain;
using Wheel.Domain.Localization;
using Wheel.Services.LocalizationManage.Dtos;

namespace Wheel.Services.LocalizationManage
{
    /// <summary>
    /// 多语言管理
    /// </summary>
    public class LocalizationManageAppService : WheelServiceBase, ILocalizationManageAppService
    {
        private readonly IBasicRepository<LocalizationCulture, int> _localizationCultureRepository;
        private readonly IBasicRepository<LocalizationResource, int> _localizationResourceRepository;

        public LocalizationManageAppService(IBasicRepository<LocalizationCulture, int> localizationCultureRepository, IBasicRepository<LocalizationResource, int> localizationResourceRepository)
        {
            _localizationCultureRepository = localizationCultureRepository;
            _localizationResourceRepository = localizationResourceRepository;
        }
        /// <summary>
        /// 获取地区多语言详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LocalizationCultureDto?> GetLocalizationCultureAsync(int id)
        {
            var entity = await _localizationCultureRepository.FindAsync(id);

            return Mapper.Map<LocalizationCultureDto>(entity);
        }
        /// <summary>
        /// 分页获取地区多语言列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Page<LocalizationCultureDto>?> GetLocalizationCulturePageListAsync(PageRequest input)
        {
            var (entities, total) = await _localizationCultureRepository
                .GetPageListAsync(a => true,
                (input.PageIndex - 1) * input.PageSize,
                input.PageSize,
                propertySelectors: a => a.Resources
                );

            return new Page<LocalizationCultureDto>(Mapper.Map<List<LocalizationCultureDto>>(entities), total);
        }
        /// <summary>
        /// 创建地区多语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<LocalizationCultureDto> CreateLocalizationCultureAsync(CreateLocalizationCultureDto input)
        {
            var entity = Mapper.Map<LocalizationCulture>(input);
            entity = await _localizationCultureRepository.InsertAsync(entity);
            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<LocalizationCultureDto>(entity);
        }
        /// <summary>
        /// 删除地区多语言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteLocalizationCultureAsync(int id)
        {
            await _localizationCultureRepository.DeleteAsync(id);
            await UnitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 创建多语言资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<LocalizationResourceDto> CreateLocalizationResourceAsync(CreateLocalizationResourceDto input)
        {
            var entity = Mapper.Map<LocalizationResource>(input);
            entity = await _localizationResourceRepository.InsertAsync(entity);
            await UnitOfWork.SaveChangesAsync();
            return Mapper.Map<LocalizationResourceDto>(entity);
        }
        /// <summary>
        /// 修改多语言资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateLocalizationResourceAsync(UpdateLocalizationResourceDto input)
        {
            await _localizationResourceRepository.UpdateAsync(a => a.Id == input.Id,
                a => a.SetProperty(b => b.Key, b => input.Key).SetProperty(b => b.Value, b => input.Value));
            await UnitOfWork.SaveChangesAsync();
        }
        /// <summary>
        /// 删除多语言资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteLocalizationResourceAsync(int id)
        {
            await _localizationResourceRepository.DeleteAsync(id);
            await UnitOfWork.SaveChangesAsync();
        }
    }
}
