using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities.Common
{
    public interface IAuditEntity
    {
        string CreationUser { get; set; }
        DateTime CreationDate { get; set; }
        string ModificationUser { get; set; }
        DateTime? ModificationDate { get; set; }
    }
}
