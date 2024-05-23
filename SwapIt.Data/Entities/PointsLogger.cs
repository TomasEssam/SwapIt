using SwapIt.Data.Entities.Common;
using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities
{
    public class PointsLogger : IDeletedEntity, IAuditEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Type { get; set; }
        public int Points { get; set; }
        public int UserId { get; set; }
        public int? ServiceRequestId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("ServiceRequestId")]
        public ServiceRequest? ServiceRequest { get; set; }
    }

}
