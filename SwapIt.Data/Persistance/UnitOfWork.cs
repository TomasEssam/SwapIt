using SwapIt.Data.Entities.Context;
using SwapIt.Data.IPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory _dbFactory;
        private SwapItDbContext _dbcontext;
        public UnitOfWork(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
            _dbcontext = _dbcontext ?? (_dbcontext = dbFactory.Init());
        }
        public SwapItDbContext DbContext
        {
            get { return _dbcontext ?? (_dbcontext = _dbFactory.Init()); }

        }

        public void SaveChanges()
        {
            _dbcontext.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return _dbcontext.SaveChangesAsync();
        }
    }
}
