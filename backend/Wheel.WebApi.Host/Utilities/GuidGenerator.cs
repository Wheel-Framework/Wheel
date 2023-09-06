using SequentialGuid;
using Wheel.DependencyInjection;

namespace Wheel.Utilities
{
    public class GuidGenerator : ISingletonDependency
    {
        public Guid Create() => SequentialGuidGenerator.Instance.NewGuid();
    }
}
