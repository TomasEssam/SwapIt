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
        public TransactionType Type { get; set; }
        public float Amount { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }

    public enum TransactionType
    {
        Deposit,
        Withdraw,
        Hold
    }
}
