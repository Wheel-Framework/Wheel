using IdGen;
using Wheel.DependencyInjection;

namespace Wheel.Utilities
{
    public class SnowflakeIdGenerator(IdGenerator idGenerator) : ISingletonDependency
    {
        public long Create()
        {
            return idGenerator.CreateId();
        }
    }
}
