using SwapIt.BL.DTOs;
using SwapIt.BL.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.IServices
{
    public interface ICategoryService
    {
        Task<bool> CreateAsync(CategoryDto dto);
        Task<bool> DeleteAsync(int categoryId);
        Task<bool> UpdateAsync(CategoryDto dto);
        Task<CategoryDto> GetByIdAsync(int categoryId);
        Task<List<DropDownDto>> GetCategoryDDAsync();
    }
}
