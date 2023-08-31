using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Wheel.Notifications;

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
                var wellcome = new NotificationData(NotificationType.WellCome)
                    .WithData("name", Context.User!.Identity!.Name!)
                    .WithData("message", L["Hello"].Value);
                await Clients.Caller.SendAsync("Notification", wellcome);
            }
        }
    }
}
