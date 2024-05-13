using AutoMapper;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.Data.Entities;
using SwapIt.Data.IRepository;
using SwapIt.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        readonly ICategoryRepository _categoryRepository;
        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryDto> GetByIdAsync(int categoryId)
        {
            var model = await _categoryRepository.GetByIdAsync(categoryId);
            return _mapper.Map<CategoryDto>(model);
        }

        public async Task<bool> CreateAsync(CategoryDto dto)
        {
            var model = _mapper.Map<Category>(dto);
            return await _categoryRepository.AddAsync(model);

        }

        public async Task<bool> DeleteAsync(int categoryId)
        {
            return await _categoryRepository.DeleteByIdAsync(categoryId);  
        }


        public async Task<List<DropDownDto>> GetCategoryDDAsync()
        {
            var model = await _categoryRepository.GetAllAsync();
            return _mapper.Map<List<DropDownDto>>(model);
        }

        public async Task<bool> UpdateAsync(CategoryDto dto)
        {
            var model = _mapper.Map<Category>(dto);
            return await _categoryRepository.UpdateAsync(model);
             
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var model = await _categoryRepository.GetAllAsync();
            return _mapper.Map<List<CategoryDto>>(model); 
        }
    }
}
