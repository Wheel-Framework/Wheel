using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Wheel.Notifications;

namespace Wheel.Hubs
{
    public class NotificationHub(IStringLocalizerFactory localizerFactory) : Hub
    {
        protected IStringLocalizer L = localizerFactory.Create(null);

        public override async Task OnConnectedAsync()
        {
            if (Context.UserIdentifier != null)
            {
                var wellcome = new NotificationData(NotificationType.WellCome)
                    .WithData("name", Context.User!.Identity!.Name!)
                    .WithData("message", L["Hello"].Value);
                await Clients.Caller.SendAsync("Notification", wellcome);
            }
        }
    }
}
