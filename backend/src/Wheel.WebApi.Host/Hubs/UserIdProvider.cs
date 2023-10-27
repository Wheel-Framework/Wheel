using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Wheel.DependencyInjection;

namespace Wheel.Hubs
{
    public class UserIdProvider : IUserIdProvider, ISingletonDependency
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Claims?.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
