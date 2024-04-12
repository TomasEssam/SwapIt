using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities.Common
{
    public interface IDeletedEntity
    {
        bool IsDeleted { get; set; }
        DateTime? DeletionDate { get; set; }
    }
}
