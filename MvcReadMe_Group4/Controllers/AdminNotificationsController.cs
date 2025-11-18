using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvcReadMe_Group4.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcReadMe_Group4.Controllers
{
    public class AdminNotificationsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminNotificationsController> _logger;
        private readonly Microsoft.AspNetCore.SignalR.IHubContext<MvcReadMe_Group4.Hubs.NotificationHub> _hubContext;

        public AdminNotificationsController(IConfiguration configuration, ILogger<AdminNotificationsController> logger, Microsoft.AspNetCore.SignalR.IHubContext<MvcReadMe_Group4.Hubs.NotificationHub> hubContext)
        {
            _configuration = configuration;
            _logger = logger;
            _hubContext = hubContext;
        }

        // GET: /AdminNotifications
        public async Task<IActionResult> Index()
        {
            var list = new List<AdminNotificationViewModel>();
            try
            {
                var mySqlConn = _configuration.GetConnectionString("MySqlConnection");
                if (!string.IsNullOrEmpty(mySqlConn))
                {
                    await using var mconn = new MySqlConnector.MySqlConnection(mySqlConn);
                    await mconn.OpenAsync();
                        try { var alt = mconn.CreateCommand(); alt.CommandText = "ALTER TABLE admin_notifications ADD COLUMN IF NOT EXISTS is_read TINYINT(1) DEFAULT 0;"; await alt.ExecuteNonQueryAsync(); } catch { }
                    var cmd = mconn.CreateCommand();
                        cmd.CommandText = "SELECT id, message, created_at, is_read FROM admin_notifications ORDER BY created_at DESC LIMIT 500";
                    await using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var id = reader.IsDBNull(0) ? 0L : reader.GetInt64(0);
                        var msg = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        System.DateTime? createdAt = null;
                        if (!reader.IsDBNull(2))
                        {
                            var obj = reader.GetValue(2);
                            if (obj is System.DateTime dtVal)
                            {
                                createdAt = dtVal;
                            }
                            else if (obj is string sVal)
                            {
                                if (System.DateTime.TryParse(sVal, out var dt2)) createdAt = dt2;
                            }
                            else if (obj is long lval)
                            {
                                try { createdAt = System.DateTimeOffset.FromUnixTimeSeconds(lval).UtcDateTime; } catch { }
                            }
                            else if (obj is int ival)
                            {
                                try { createdAt = System.DateTimeOffset.FromUnixTimeSeconds(ival).UtcDateTime; } catch { }
                            }
                        }
                        // is_read is at index 3 (0-based)
                        var isRead = false;
                        try { isRead = !reader.IsDBNull(3) && (reader.GetInt32(3) != 0); } catch { }
                        list.Add(new AdminNotificationViewModel { Id = id, Message = msg, CreatedAt = createdAt });
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning(ex, "Failed to load admin notifications from MySQL");
            }

            return View(list);
        }

        // GET: /AdminNotifications/GetCount
        [HttpGet]
        public async Task<IActionResult> GetCount()
        {
            int cnt = 0;
            try
            {
                var mySqlConn = _configuration.GetConnectionString("MySqlConnection");
                if (!string.IsNullOrEmpty(mySqlConn))
                {
                    await using var mconn = new MySqlConnector.MySqlConnection(mySqlConn);
                    await mconn.OpenAsync();
                    // ensure is_read column exists so count query works
                    try { var alt = mconn.CreateCommand(); alt.CommandText = "ALTER TABLE admin_notifications ADD COLUMN IF NOT EXISTS is_read TINYINT(1) DEFAULT 0;"; await alt.ExecuteNonQueryAsync(); } catch { }
                    var cmd = mconn.CreateCommand();
                    cmd.CommandText = "SELECT COUNT(*) FROM admin_notifications WHERE is_read = 0";
                    var res = await cmd.ExecuteScalarAsync();
                    if (res != null && int.TryParse(res.ToString(), out var v)) cnt = v;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning(ex, "Failed to count admin notifications");
            }

            return Json(new { count = cnt });
        }

        // POST: /AdminNotifications/MarkAllRead
        [HttpPost]
        public async Task<IActionResult> MarkAllRead()
        {
            try
            {
                var mySqlConn = _configuration.GetConnectionString("MySqlConnection");
                if (!string.IsNullOrEmpty(mySqlConn))
                {
                    await using var mconn = new MySqlConnector.MySqlConnection(mySqlConn);
                    await mconn.OpenAsync();
                    try { var alt = mconn.CreateCommand(); alt.CommandText = "ALTER TABLE admin_notifications ADD COLUMN IF NOT EXISTS is_read TINYINT(1) DEFAULT 0;"; await alt.ExecuteNonQueryAsync(); } catch { }
                    var upd = mconn.CreateCommand();
                    upd.CommandText = "UPDATE admin_notifications SET is_read = 1 WHERE is_read = 0";
                    await upd.ExecuteNonQueryAsync();

                    // broadcast new count (should be 0)
                    try { await _hubContext.Clients.Group("admins").SendAsync("AdminNotificationCount", 0); } catch { }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning(ex, "Failed to mark all admin notifications read");
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        // POST: /AdminNotifications/MarkRead
        [HttpPost]
        public async Task<IActionResult> MarkRead(long id)
        {
            try
            {
                var mySqlConn = _configuration.GetConnectionString("MySqlConnection");
                if (!string.IsNullOrEmpty(mySqlConn))
                {
                    await using var mconn = new MySqlConnector.MySqlConnection(mySqlConn);
                    await mconn.OpenAsync();
                    try { var alt = mconn.CreateCommand(); alt.CommandText = "ALTER TABLE admin_notifications ADD COLUMN IF NOT EXISTS is_read TINYINT(1) DEFAULT 0;"; await alt.ExecuteNonQueryAsync(); } catch { }
                    var upd = mconn.CreateCommand();
                    upd.CommandText = "UPDATE admin_notifications SET is_read = 1 WHERE id = @id";
                    upd.Parameters.AddWithValue("@id", id);
                    await upd.ExecuteNonQueryAsync();

                    // compute remaining unread count and broadcast
                    var cntcmd = mconn.CreateCommand(); cntcmd.CommandText = "SELECT COUNT(*) FROM admin_notifications WHERE is_read = 0";
                    var res = await cntcmd.ExecuteScalarAsync(); int remaining = 0; if (res != null && int.TryParse(res.ToString(), out var v)) remaining = v;
                    try { await _hubContext.Clients.Group("admins").SendAsync("AdminNotificationCount", remaining); } catch { }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning(ex, "Failed to mark admin notification read");
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }
    }
}
