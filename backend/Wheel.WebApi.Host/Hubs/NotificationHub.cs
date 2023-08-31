using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;

namespace Wheel.Hubs
{
    public class NotificationHub : Hub
    {
        protected IStringLocalizer L;

        public NotificationHub(IStringLocalizerFactory localizerFactory)
        {
            L = localizerFactory.Create(null);
        }

        public override async Task OnConnectedAsync()
        {
            if(Context.UserIdentifier != null)
            {
                await Clients.Caller.SendAsync("Notification", Context.User.Identity.Name, L["Hello"].Value);
            }
        }
    }
}
