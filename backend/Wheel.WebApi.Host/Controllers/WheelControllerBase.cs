using Microsoft.AspNetCore.Mvc;
using Wheel.Utilities;

namespace Wheel.Controllers
{
    public abstract class WheelControllerBase : ControllerBase
    {
        public SnowflakeIdGenerator SnowflakeIdGenerator { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
    }
}
