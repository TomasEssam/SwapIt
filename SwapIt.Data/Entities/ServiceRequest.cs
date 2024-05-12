using Microsoft.Extensions.DependencyInjection;
using SwapIt.Data.Constants;
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
    public class ServiceRequest : IDeletedEntity, IAuditEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        [Required]
        public string RequestState { get; set; } 
        public float? ExecutionTime { get; set; }
        [MaxLength(500)]
        public string? Notes { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
        [ForeignKey("CustomerId")]
        public ApplicationUser Customer { get; set; }
    }
}
