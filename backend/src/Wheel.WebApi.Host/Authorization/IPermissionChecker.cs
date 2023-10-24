namespace Wheel.Authorization
{
    public interface IPermissionChecker
    {
        Task<bool> Check(string controller, string action);
    }
}
