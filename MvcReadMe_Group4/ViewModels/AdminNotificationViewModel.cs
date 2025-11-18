using System;

namespace MvcReadMe_Group4.ViewModels
{
    public class AdminNotificationViewModel
    {
        public long Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
    }
}
