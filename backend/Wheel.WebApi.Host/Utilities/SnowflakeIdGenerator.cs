using IdGen;
using Wheel.DependencyInjection;

namespace Wheel.Utilities
{
    public class SnowflakeIdGenerator : ISingletonDependency
    {
        private IdGenerator IdGenerator;

        public SnowflakeIdGenerator(IdGenerator idGenerator)
        {
            IdGenerator = idGenerator;
        }

        public long Create()
        {
            return IdGenerator.CreateId();
        }
    }
}
