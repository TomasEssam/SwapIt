using SwapIt.Data.Entities.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.IPersistance
{
    public interface IDbFactory : IDisposable
    {
        SwapItDbContext Init();
    }
}
