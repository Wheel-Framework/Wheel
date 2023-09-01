namespace Wheel.Authorization
{
    public interface IPermissionChecker
    {
        bool Check(string controller, string action);
    }
}
