using Wheel.Utilities;

namespace Wheel.Services
{
    public abstract class WheelServiceBase
    {
        public SnowflakeIdGenerator SnowflakeIdGenerator { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }
    }
}
