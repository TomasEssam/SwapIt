using Microsoft.EntityFrameworkCore;
using SwapIt.Data.Entities.Context;
using SwapIt.Data.IPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Persistance
{
    public class DbFactory : Disposable, IDbFactory
    {
        DbContextOptions<SwapItDbContext> _options;
        public DbFactory(DbContextOptions<SwapItDbContext> options)
        {
            _options = options;
        }
        SwapItDbContext dbContext;

        public SwapItDbContext Init()
        {
            return dbContext ?? (dbContext = new SwapItDbContext(_options));
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
