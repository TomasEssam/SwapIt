using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities
{
    public class UserNotificationDto
    {
        public int UserNotificationId { get; set; }
        public bool IsRead { get; set; }
        public int ApplicationUserId { get; set; }
        public string? NotificationType { get; set; }
        public string? Content { get; set; }
    }
}
