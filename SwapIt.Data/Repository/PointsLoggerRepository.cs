using Microsoft.EntityFrameworkCore;
using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Context;
using SwapIt.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Repository
{
    public class PointsLoggerRepository : IPointsLoggerRepository
    {
        private readonly SwapItDbContext Context;

        public PointsLoggerRepository(SwapItDbContext context)
        {
            Context = context;
        }

        public async Task<bool> AddAsync(PointsLogger pointsLogger)
        {
            Context.PointsLoggers.Add(pointsLogger);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(PointsLogger pointsLogger)
        {
            pointsLogger.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var pointsLogger = await GetByIdAsync(id);
            if (pointsLogger == null)
                return false;

            pointsLogger.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<PointsLogger>> GetAllAsync()
        {
            return await Context.PointsLoggers.ToListAsync();
        }

        public async Task<PointsLogger> GetByIdAsync(int id)
        {
            return await Context.PointsLoggers.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(PointsLogger newPointsLogger)
        {
            var pointsLogger = await GetByIdAsync(newPointsLogger.Id);
            if (pointsLogger == null)
            {
                return false;
            }

            pointsLogger.Type = newPointsLogger.Type;
            pointsLogger.Points = newPointsLogger.Points;
            pointsLogger.ModificationDate = DateTime.Now;

            await Context.SaveChangesAsync();
                return true;
            
        }


        public async Task<PointsLogger> GetByServiceRequestIdAsync(int requestId)
        {
            return await Context.PointsLoggers.FirstOrDefaultAsync(c => c.ServiceRequestId == requestId);
        }
    }
}
