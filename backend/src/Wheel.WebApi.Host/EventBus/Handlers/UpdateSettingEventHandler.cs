using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Wheel.DependencyInjection;
using Wheel.Domain.Settings;
using Wheel.EventBus.Distributed;
using Wheel.EventBus.EventDatas;
using Wheel.Services.SettingManage.Dtos;

namespace Wheel.EventBus.Handlers
{
    public class UpdateSettingEventHandler(SettingManager settingManager, IDistributedCache distributedCache,
            IMapper mapper)
        : IDistributedEventHandler<UpdateSettingEventData>, ITransientDependency
    {
        public async Task Handle(UpdateSettingEventData eventData, CancellationToken cancellationToken = default)
        {
            var settings = await settingManager.GetSettingValues(eventData.GroupName, eventData.SettingScope, eventData.SettingScopeKey, cancellationToken);

            await distributedCache.SetAsync($"Setting:{eventData.GroupName}:{eventData.SettingScope}" + (eventData.SettingScope == Enums.SettingScope.Global ? "" : $":{eventData.SettingScopeKey}"), mapper.Map<List<SettingValueDto>>(settings));
        }
    }
}
