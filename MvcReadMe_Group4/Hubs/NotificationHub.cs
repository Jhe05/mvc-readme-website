using Microsoft.AspNetCore.SignalR;
using MvcReadMe_Group4.Data;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace MvcReadMe_Group4.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly MvcReadMe_Group4Context _context;

        public NotificationHub(MvcReadMe_Group4Context context)
        {
            _context = context;
        }

        public async Task SendNotification(string user, string message)
        {
            // Legacy method: broadcast to all (kept for compatibility)
            await Clients.All.SendAsync("ReceiveNotification", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var http = Context.GetHttpContext();
                if (http != null)
                {
                    // Try to read session-based UserId and determine role
                    var sidObj = http.Session.GetInt32("UserId");
                    if (sidObj.HasValue)
                    {
                        var uid = sidObj.Value;
                        var user = await _context.Users.FindAsync(uid);
                        if (user != null && (user.Role ?? "User").Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        {
                            await Groups.AddToGroupAsync(Context.ConnectionId, "admins");
                        }
                    }
                }
            }
            catch (Exception)
            {
                // swallow — connecting should not fail because of group assignment
            }

            await base.OnConnectedAsync();
        }
    }
}