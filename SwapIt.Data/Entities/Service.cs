using SwapIt.Data.Entities.Common;
using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities
{
    public class Service : IDeletedEntity, IAuditEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int TimeToExecute { get; set; }
        public string PreviousworkImagesUrl { get; set; }
        public int ServiceProviderId { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [ForeignKey("ServiceProviderId")]
        public ApplicationUser ServiceProvider { get; set; }
        public ICollection<Rate> Rates { get; set; }
        public ICollection<ServiceRequest> ServiceRequests { get; set; }
    }
}
