using Microsoft.AspNetCore.Identity;
using SwapIt.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities.Identity
{
    public class ApplicationUserRole : IdentityUserRole<int>, IDeletedEntity, IAuditEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
