using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Wheel.Uow;
using Wheel.Utilities;

namespace Wheel.Services
{
    public abstract class WheelServiceBase
    {
        public IServiceProvider ServiceProvider { get; set; }
        public SnowflakeIdGenerator SnowflakeIdGenerator => LazyGetService<SnowflakeIdGenerator>();
        public IHttpContextAccessor HttpContextAccessor => LazyGetService<IHttpContextAccessor>();

        public IUnitOfWork UnitOfWork => LazyGetService<IUnitOfWork>();

        public IMapper Mapper =>  LazyGetService<IMapper>();
        public IMemoryCache MemoryCache => LazyGetService<IMemoryCache>();

        public IDistributedCache DistributedCache => LazyGetService<IDistributedCache>();
        public IStringLocalizerFactory LocalizerFactory => LazyGetService<IStringLocalizerFactory>();

        private IStringLocalizer _stringLocalizer = null;

        public IStringLocalizer L { 
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
