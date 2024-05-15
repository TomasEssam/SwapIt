using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        [Required]
        public string NotificationType { get; set; }
        [MaxLength(1000)]
        public string Content { get; set; }
    }
}
