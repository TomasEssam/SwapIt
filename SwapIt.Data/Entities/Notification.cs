using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string NotificationType { get; set; }
        public string Content { get; set; }
        public bool IsReaded { get; set; }
        // I want it to be for all users
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
