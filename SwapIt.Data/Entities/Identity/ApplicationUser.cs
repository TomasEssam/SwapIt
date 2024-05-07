using Microsoft.AspNetCore.Identity;
using SwapIt.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>, IDeletedEntity, IAuditEntity
    {
        public Guid ApplicationUserId { get; set; }
        #region new fields 
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        #endregion
        public string? Address { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? ImageId { get; set; } 
        public string? RefreshToken { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }

    }
}
