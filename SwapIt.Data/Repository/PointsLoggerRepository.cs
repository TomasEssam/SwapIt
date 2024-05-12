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

        public async Task<PointsLogger> AddAsync(PointsLogger pointsLogger)
        {
            Context.PointsLoggers.Add(pointsLogger);
            await Context.SaveChangesAsync();
            return pointsLogger;
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
            return await Context.PointsLoggers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<PointsLogger> UpdateAsync(PointsLogger newPointsLogger)
        {
            var pointsLogger = await GetByIdAsync(newPointsLogger.Id);
            if (pointsLogger == null)
            {
                return null;
            }
            else
            {
                //we need to use Auto mapper
                throw new Exception();
                pointsLogger = newPointsLogger;
                await Context.SaveChangesAsync();
                return newPointsLogger;
            }
        }
    }
}
