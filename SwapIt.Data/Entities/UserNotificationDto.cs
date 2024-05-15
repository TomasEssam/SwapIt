using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities
{
    public class UserNotificationDto
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }
        public int ApplicationUserId { get; set; }
        public int NotificationId { get; set; }
    }
}
