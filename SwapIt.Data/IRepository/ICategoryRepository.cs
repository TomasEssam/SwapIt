using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.IRepository
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<bool> AddAsync(Category entity);
        Task<bool> UpdateAsync(Category entity);
        Task<bool> DeleteAsync(Category entity);
        Task<bool> DeleteByIdAsync(int id);
    }
}
