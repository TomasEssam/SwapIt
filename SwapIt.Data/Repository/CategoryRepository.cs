using Microsoft.EntityFrameworkCore;
using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Common;
using SwapIt.Data.Entities.Context;
using SwapIt.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SwapItDbContext Context;

        public CategoryRepository(SwapItDbContext swapItDbContext)
        {
            Context = swapItDbContext ?? throw new ArgumentNullException(nameof(swapItDbContext));
        }
        public async Task<Category> AddAsync(Category category)
        {
            Context.Categories.Add(category);
            await Context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(Category category)
        {
            category.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category == null)
                return false;

            category.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await Context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await Context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
        //Ask about it
        public async Task<Category> UpdateAsync(Category newCategory)
        {
            var category = await GetByIdAsync(newCategory.Id);
            if (category == null)
            {
                return null;
            }
            else
            {
                //we need to use Auto mapper
                throw new Exception();
                category = newCategory;
                await Context.SaveChangesAsync();
                return newCategory;
            }
        }
    }
}
