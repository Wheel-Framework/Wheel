using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Wheel.Core.Users;
using Wheel.EventBus.Local;
using Wheel.Uow;
using Wheel.Utilities;

namespace Wheel.Controllers
{
    [Authorize("Permission")]
    public abstract class WheelControllerBase : ControllerBase
    {
        public IServiceProvider ServiceProvider { get; set; }
        public SnowflakeIdGenerator SnowflakeIdGenerator => LazyGetService<SnowflakeIdGenerator>();
        public IUnitOfWork UnitOfWork => LazyGetService<IUnitOfWork>();
        public IMapper Mapper => LazyGetService<IMapper>();
        public IMemoryCache MemoryCache => LazyGetService<IMemoryCache>();

        public IDistributedCache DistributedCache => LazyGetService<IDistributedCache>();
        public ILocalEventBus LocalEventBus => LazyGetService<ILocalEventBus>();
        public ICurrentUser CurrentUser => LazyGetService<ICurrentUser>();
        public IStringLocalizerFactory LocalizerFactory => LazyGetService<IStringLocalizerFactory>();


        private IStringLocalizer _stringLocalizer = null;

        public IStringLocalizer L
        {
            get
            {
                if (_stringLocalizer == null)
                    _stringLocalizer = LocalizerFactory.Create(null);
                return _stringLocalizer;
            }
        }

        public T LazyGetService<T>() where T : notnull
        {
            return new Lazy<T>(ServiceProvider.GetRequiredService<T>).Value;
        }
    }
}
