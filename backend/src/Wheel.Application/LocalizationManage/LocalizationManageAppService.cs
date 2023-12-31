﻿using Wheel.Core.Dto;
using Wheel.Domain;
using Wheel.Domain.Localization;
using Wheel.Services.LocalizationManage.Dtos;

namespace Wheel.Services.LocalizationManage
{
    /// <summary>
    /// 多语言管理
    /// </summary>
    public class LocalizationManageAppService(IBasicRepository<LocalizationCulture, int> localizationCultureRepository,
            IBasicRepository<LocalizationResource, int> localizationResourceRepository)
        : WheelServiceBase, ILocalizationManageAppService
    {
        /// <summary>
        /// 获取地区多语言详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<R<LocalizationCultureDto>> GetLocalizationCultureAsync(int id)
        {
            var entity = await localizationCultureRepository.FindAsync(id);

            return Success(Mapper.Map<LocalizationCultureDto>(entity));
        }
        /// <summary>
        /// 分页获取地区多语言列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Page<LocalizationCultureDto>> GetLocalizationCulturePageListAsync(PageRequest input)
        {
            var (entities, total) = await localizationCultureRepository
                .GetPageListAsync(a => true,
                (input.PageIndex - 1) * input.PageSize,
                input.PageSize,
                propertySelectors: a => a.Resources
                );

            return Page(Mapper.Map<List<LocalizationCultureDto>>(entities), total);
        }
        /// <summary>
        /// 创建地区多语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<R<LocalizationCultureDto>> CreateLocalizationCultureAsync(CreateLocalizationCultureDto input)
        {
            var entity = Mapper.Map<LocalizationCulture>(input);
            entity = await localizationCultureRepository.InsertAsync(entity);
            await UnitOfWork.SaveChangesAsync();
            return Success(Mapper.Map<LocalizationCultureDto>(entity));
        }
        /// <summary>
        /// 删除地区多语言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<R> DeleteLocalizationCultureAsync(int id)
        {
            await localizationCultureRepository.DeleteAsync(id);
            await UnitOfWork.SaveChangesAsync();
            return Success();
        }
        /// <summary>
        /// 创建多语言资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<R<LocalizationResourceDto>> CreateLocalizationResourceAsync(CreateLocalizationResourceDto input)
        {
            var entity = Mapper.Map<LocalizationResource>(input);
            entity = await localizationResourceRepository.InsertAsync(entity);
            await UnitOfWork.SaveChangesAsync();
            return Success(Mapper.Map<LocalizationResourceDto>(entity));
        }
        /// <summary>
        /// 修改多语言资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<R> UpdateLocalizationResourceAsync(UpdateLocalizationResourceDto input)
        {
            await localizationResourceRepository.UpdateAsync(a => a.Id == input.Id,
                a => a.SetProperty(b => b.Key, b => input.Key)
                .SetProperty(b => b.Value, b => input.Value));
            await UnitOfWork.SaveChangesAsync();
            return Success();
        }
        /// <summary>
        /// 删除多语言资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<R> DeleteLocalizationResourceAsync(int id)
        {
            await localizationResourceRepository.DeleteAsync(id);
            await UnitOfWork.SaveChangesAsync();
            return Success();
        }
    }
}
