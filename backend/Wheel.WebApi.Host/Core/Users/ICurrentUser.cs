using Wheel.DependencyInjection;

namespace Wheel.Core.Users
{
    public interface ICurrentUser : IScopeDependency
    {
        bool IsAuthenticated { get; }

        string UserName { get; }

        string[] Roles { get; }
    }
}
