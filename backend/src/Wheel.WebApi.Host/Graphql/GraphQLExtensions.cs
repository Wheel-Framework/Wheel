using HotChocolate.Execution.Configuration;
using System.Reflection;

namespace Wheel.Graphql
{
    public static class GraphQLExtensions
    {
        public static IRequestExecutorBuilder AddWheelGraphQL(this IServiceCollection services)
        {
            var result = services.AddGraphQLServer()
            .AddAuthorization()
            .AddQueryType<Query>()
            ;

            var abs = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                        .Where(x => !x.Contains("Microsoft.") && !x.Contains("System."))
                        .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x))).ToArray();
            var types = abs.SelectMany(ab => ab.GetTypes()
                .Where(t => typeof(IQueryExtendObjectType).IsAssignableFrom(t) && typeof(IQueryExtendObjectType) != t));
            if (types.Any())
            {
                result = result.AddTypes(types.ToArray());
            }
            return result;
        }
    }
}
