using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Wheel.DependencyInjection;
using Wheel.Domain.Settings;
using Wheel.EventBus.Distributed;
using Wheel.EventBus.EventDatas;
using Wheel.Services.SettingManage.Dtos;

namespace Wheel.EventBus.Handlers
{
    public class UpdateSettingEventHandler : IDistributedEventHandler<UpdateSettingEventData>, ITransientDependency
    {

        private readonly SettingManager _settingManager;
        private readonly IDistributedCache _distributedCache;
        private readonly IMapper _mapper;

        public UpdateSettingEventHandler(SettingManager settingManager, IDistributedCache distributedCache, IMapper mapper)
        {
            _settingManager = settingManager;
            _distributedCache = distributedCache;
            _mapper = mapper;
        }

        public async Task Handle(UpdateSettingEventData eventData, CancellationToken cancellationToken = default)
        {
            var settings = await _settingManager.GetSettingValues(eventData.GroupName, eventData.SettingScope, eventData.SettingScopeKey, cancellationToken);
            
            await _distributedCache.SetAsync($"Setting:{eventData.GroupName}:{eventData.SettingScope}" + (eventData.SettingScope == Enums.SettingScope.Golbal ? "" : $":{eventData.SettingScopeKey}"), _mapper.Map<List<SettingValueDto>>(settings));
        }
    }
}
