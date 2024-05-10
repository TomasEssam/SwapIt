using Microsoft.Extensions.DependencyInjection;
using SwapIt.Data.Entities.Common;
using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities
{
    public class UserNotification : IDeletedEntity, IAuditEntity
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }
        public int ApplicationUserId { get; set; }
        public int NotificationId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser Users { get; set; }

        [ForeignKey("NotificationId")]
        public Notification Notifications { get; set; }
    }
}
