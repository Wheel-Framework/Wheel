using Wheel.DependencyInjection;

namespace Wheel.Authorization
{
    public class PermissionChecker : IPermissionChecker, ITransientDependency
    {
        public bool Check(string controller, string action)
        {
            return true;
        }
    }
}
