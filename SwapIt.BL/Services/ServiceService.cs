using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Context;
using SwapIt.Data.Entities.Identity;
using SwapIt.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IMapper _mapper;
        readonly IServiceRepository _serviceRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly UserManager<ApplicationUser> _userManager;
        readonly SwapItDbContext _context;

        public ServiceService(IMapper mapper, IServiceRepository serviceRepository, SwapItDbContext context, ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _serviceRepository = serviceRepository;
            _context = context;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }
        public async Task<ServiceDto> GetServiceByIdAsync(int serviceId)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId);
            var model = _mapper.Map<ServiceDto>(service);
            return model;
        }
        public async Task<bool> CreateAsync(ServiceDto dto)
        {
            var model = _mapper.Map<Service>(dto);
            return await _serviceRepository.AddAsync(model);

        }

        public async Task<bool> DeleteAsync(int serviceId)
        {
            return await _serviceRepository.DeleteByIdAsync(serviceId);
            
        }

        public async Task<List<ServiceDto>> GetAllAcceptedAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ServiceDto>> GetAllAsync()
        {
            var model = await _serviceRepository.GetAllAsync();
            return _mapper.Map<List<ServiceDto>>(model);
        }

        public async Task<List<ServiceDto>> GetAllFinshiedAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetAllPreviousWorkImageUrlAsync(int userId)
        {
            var servicesList = await _serviceRepository.GetAllAsync();
            var userServicesUrls = servicesList.Where(x => x.ServiceProviderId == userId)
                .Select(x => x.PreviousworkImagesUrl).ToList();
            return userServicesUrls;
        }



        public async Task<List<SearchResultDto>> SearchServiceAsync(ServiceFilterDto dto)
        {
            var query = _context.Services.AsQueryable();
            var services = new List<Service>();
            if (String.IsNullOrEmpty(dto.ServiceName))
            {
                query = _context.Services.Where(x => x.Name.Contains(dto.ServiceName));
            }
            if (dto.ServicePrice > 0)
            {
                query = _context.Services.Where(x => x.Price <= dto.ServicePrice);
            }
            if (dto.ServiceProviderId > 0)
            {
                query = _context.Services.Where(x => x.ServiceProviderId == dto.ServiceProviderId);
            }
            if (dto.CategoryId > 0)
            {
                query = _context.Services.Where(x => x.CategoryId == dto.CategoryId);
            }
            services = query.ToList();


            var result = new List<SearchResultDto>();
            SearchResultDto serviceWithProvidersDTO;

            
            foreach (var service in services)
            {
                var category = await _categoryRepository.GetByIdAsync(service.CategoryId);
                var user = await _userManager.FindByIdAsync(service.ServiceProviderId.ToString());

                serviceWithProvidersDTO = new SearchResultDto()
                {
                    ServiceId = service.Id,
                    ServiceName = service.Name,
                    ServiceDescription = service.Description,
                    ServicePrice = service.Price,
                    CategoryName = category.Name,
                    Username = user.UserName,
                    ProfileImagePath = user.ProfileImagePath,
                    //totalRate 
                };
            }
            

            return result;





            //return _mapper.Map<List<ServiceDto>>(services);
        }

        public async Task<bool> UpdateAsync(ServiceDto dto)
        {
            var model = _mapper.Map<Service>(dto);
            return await _serviceRepository.UpdateAsync(model);
        }

        public async Task<List<DropDownDto>> GetServiceDDAsync()
        {
            var model = await _serviceRepository.GetAllAsync();
            return _mapper.Map<List<DropDownDto>>(model);
        }


    }
}
