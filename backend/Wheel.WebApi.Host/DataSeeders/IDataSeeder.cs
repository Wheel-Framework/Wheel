using Wheel.DependencyInjection;

namespace Wheel.DataSeeders
{
    public interface IDataSeeder : ITransientDependency
    {
        Task Seed(CancellationToken cancellationToken = default);
    }
}
