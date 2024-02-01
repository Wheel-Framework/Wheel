using Wheel.DependencyInjection;
using Yitter.IdGenerator;

namespace Wheel.Utilities
{
    public class SnowflakeIdGenerator : ISingletonDependency
    {
        public SnowflakeIdGenerator()
        {
            var options = new IdGeneratorOptions();
            options.WorkerId = 1; 
            options.SeqBitLength = 10; 

            YitIdHelper.SetIdGenerator(options);
        }

        public long Create()
        {
            return YitIdHelper.NextId();
        }
    }
}
