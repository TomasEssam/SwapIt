using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.Data.Constants;
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
        #region fields&cotr
        private readonly IMapper _mapper;
        private readonly IServiceRepository _serviceRepository;
        private readonly IRateRepository _rateRepository;
        private readonly SwapItDbContext _context;
        private readonly INotificationService _notificationService;

        public ServiceService(IMapper mapper, IServiceRepository serviceRepository, SwapItDbContext context, IRateRepository rateRepository, INotificationService notificationService)
        {
            _mapper = mapper;
            _serviceRepository = serviceRepository;
            _context = context;
            _rateRepository = rateRepository;
            _notificationService = notificationService;
        }
        #endregion

        public async Task<ServiceDto> GetServiceByIdAsync(int serviceId)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId);
            var model = _mapper.Map<ServiceDto>(service);
            return model;
        }
        public async Task<bool> CreateAsync(ServiceDto dto)
        {
            if (dto.serviceImage != null)
                if (!AllowedExtensions.isValid(Path.GetExtension(dto.serviceImage.FileName)))
                {
                    throw new Exception("the given file is not an image");
                }

            var model = _mapper.Map<Service>(dto);

            //for Notification 
            await _notificationService.CreateAsync(new NotificationDto
            {
                Content = $"You have been added a service {dto.Name} to your profile",
                NotificationType = NotificationTypes.CreateService
            }, dto.ServiceProviderId);

            var result = await _serviceRepository.AddAsync(model);
            if (!result)
                return false;
            if (dto.serviceImage != null)
            {
                result = await UploadServiceImage(dto.serviceImage, model.Id, FolderName.servicesImages);
                if (!result)
                    return false;
            }
            return true;
        }

        public async Task<bool> DeleteAsync(int serviceId)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId);
            await _notificationService.CreateAsync(new NotificationDto
            {
                Content = $"Your service {service.Name} have been deleted from your profile",
                NotificationType = NotificationTypes.CreateService
            }, service.ServiceProviderId);
            return await _serviceRepository.DeleteByIdAsync(serviceId);

        }

        public async Task<List<SearchResultDto>> GetAllAcceptedServiceProviderSideAsync(int serviceProviderId)
        {
            return await GetALLServiceProviderSideAsync(serviceProviderId, RequestStateNames.Accepted);

        }

        public async Task<List<ServiceDto>> GetAllAsync()
        {
            var model = await _serviceRepository.GetAllAsync();
            return _mapper.Map<List<ServiceDto>>(model);
        }

        public async Task<List<SearchResultDto>> GetAllFinshiedServiceProviderSideAsync(int serviceProviderId)
        {
            return await GetALLServiceProviderSideAsync(serviceProviderId, RequestStateNames.Finished);
        }

        public async Task<List<ImageResultDto>> GetAllPreviousWorkImageUrlAsync(int serviceProviderId)
        {
            var servicesList = await _serviceRepository.GetAllAsync();
            var userServicesUrls = servicesList.Where(x => x.ServiceProviderId == serviceProviderId)
                .Select(x => x.PreviousworkImagesUrl).ToList();

            var imageResults = new List<ImageResultDto>();

            foreach (var imageUrl in userServicesUrls)
            {
                try
                {
                    using var imageFileStream = new FileStream(imageUrl, FileMode.Open, FileAccess.Read);
                    using var memoryStream = new MemoryStream();
                    await imageFileStream.CopyToAsync(memoryStream);
                    var base64Image = Convert.ToBase64String(memoryStream.ToArray());
                    var contentType = GetContentType(imageUrl);

                    imageResults.Add(new ImageResultDto
                    {
                        Base64Image = base64Image,
                        ContentType = contentType,
                        Success = true
                    });
                }
                catch (Exception ex)
                {
                    imageResults.Add(new ImageResultDto
                    {
                        Success = false,
                        ErrorMessage = ex.Message
                    });
                }
            }

            return imageResults;
        }



        public async Task<List<SearchResultDto>> SearchServiceAsync(ServiceFilterDto dto)
        {
            var query = _context.Services.Include(x => x.ServiceProvider)
                .Include(x => x.Category)
                .Include(x => x.Rates)
                .AsQueryable();
            var services = new List<Service>();
            if (!String.IsNullOrEmpty(dto.ServiceName))
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

            var result = query.Where(x => x.ServiceProviderId != dto.UserId).Select(x => new SearchResultDto
            {
                Id = x.Id,
                ServiceName = x.Name,
                ServiceDescription = x.Description,
                ServicePrice = x.Price,
                CategoryName = x.Category.Name,
                Username = x.ServiceProvider.UserName,
                ProfileImagePath = x.ServiceProvider.ProfileImagePath,
                totalRate = (x.Rates.Count() == 0) ? 0 : x.Rates.Select(x => x.RateValue).Sum() / x.Rates.Count(),
            }).ToList();

            return result;

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

        public async Task<List<RateDto>> GetRatesAsync(int serviceId)
        {
            return await _context.Rates.Where(r => r.ServiceId == serviceId)
                .Select(x => new RateDto
                {
                    ServiceId = x.ServiceId,
                    Id = x.Id,
                    Feedback = x.Feedback,
                    CustomerName = x.Customer.UserName,
                    RateDate = x.RateDate,
                    RateValue = x.RateValue
                }).ToListAsync();
        }

        public async Task<List<SearchResultDto>> GetAllPendingServiceProviderSideAsync(int serviceProviderId)
        {
            return await GetALLServiceProviderSideAsync(serviceProviderId, RequestStateNames.Pending);
        }

        public async Task<List<SearchResultDto>> GetAllCanceledServiceProviderSideAsync(int serviceProviderId)
        {
            return await GetALLServiceProviderSideAsync(serviceProviderId, RequestStateNames.Canceled);

        }

        public async Task<List<SearchResultDto>> GetAllAcceptedCustomerSideAsync(int customerId)
        {
            return await GetALLCustomerSideAsync(customerId, RequestStateNames.Accepted);
        }

        public async Task<List<SearchResultDto>> GetAllFinishiedCustomerSideAsync(int customerId)
        {
            return await GetALLCustomerSideAsync(customerId, RequestStateNames.Finished);
        }

        public async Task<List<SearchResultDto>> GetAllPendingCustomerSideAsync(int customerId)
        {
            return await GetALLCustomerSideAsync(customerId, RequestStateNames.Pending);
        }

        public async Task<List<SearchResultDto>> GetAllCanceledCustomerSideAsync(int customerId)
        {
            return await GetALLCustomerSideAsync(customerId, RequestStateNames.Canceled);
        }

        public async Task<bool> UploadServiceImage(IFormFile serviceImage, int serviceId, string folderName)
        {
            //folder path
            StringBuilder fullPath = new StringBuilder();
            fullPath.Append(Directory.GetCurrentDirectory());
            fullPath.Append(@"\wwwroot\");
            fullPath.Append(folderName);
            fullPath.Append(@"\");

            Directory.CreateDirectory(fullPath.ToString());

            //image details
            fullPath.Append(Guid.NewGuid().ToString());
            fullPath.Append('_');
            fullPath.Append(serviceId.ToString());
            fullPath.Append(Path.GetExtension(serviceImage.FileName));

            string imagePath = fullPath.ToString();

            try
            {
                using (var fileStream = new FileStream(fullPath.ToString(), FileMode.Create))
                {
                    await serviceImage.CopyToAsync(fileStream);
                }
                var service = await _context.Services.FindAsync(serviceId);
                if (service != null)
                {
                    service.PreviousworkImagesUrl = imagePath;
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ImageResultDto> GetServiceImage(int serviceId)
        {

            var service = await _context.Services.FindAsync(serviceId);
            if (service == null || string.IsNullOrEmpty(service.PreviousworkImagesUrl))
            {
                return new ImageResultDto
                {
                    Success = false,
                    ErrorMessage = "Service not found or image path is empty"
                };
            }

            var imagePath = service.PreviousworkImagesUrl;
            try
            {
                var imageFileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                var contentType = GetContentType(imagePath);

                return new ImageResultDto
                {
                    ImageStream = imageFileStream,
                    ContentType = contentType,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ImageResultDto
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public string GetServiceImageBase64(string ImageUrl)
        {
            using var imageFileStream = new FileStream(ImageUrl, FileMode.Open, FileAccess.Read);
            using var memoryStream = new MemoryStream();
            imageFileStream.CopyTo(memoryStream);
            return Convert.ToBase64String(memoryStream.ToArray());

        }

        private string GetContentType(string path)
        {
            var types = new Dictionary<string, string>
                        {
                             {".jpg", "image/jpeg"},
                             {".jpeg", "image/jpeg"},
                             {".png", "image/png"},
                             {".gif", "image/gif"}
                         };
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private async Task<List<SearchResultDto>> GetALLServiceProviderSideAsync(int serviceProviderId, string requestState)
        {
            return await _context.ServiceRequests.Include(x => x.Service)
                .ThenInclude(x => x.Category)
                .Include(x => x.Service)
                .ThenInclude(x => x.ServiceProvider)
                .Include(x => x.Service)
                .ThenInclude(x => x.Rates)
            .Include(x => x.Customer)
                .Where(x => x.RequestState == requestState && x.Service.ServiceProviderId == serviceProviderId)
                .Select(x => new SearchResultDto
                {
                    Id = x.ServiceId,
                    CategoryName = x.Service.Category.Name,
                    ProfileImagePath = x.Service.ServiceProvider.ProfileImagePath,
                    ServiceDescription = x.Service.Description,
                    ServiceName = x.Service.Name,
                    ServicePrice = x.Service.Price,
                    totalRate = (x.Service.Rates.Count() == 0) ? 0 : (float)x.Service.Rates.Select(x => x.RateValue).Sum() / (float)x.Service.Rates.Count(),
                    Username = x.Customer.UserName,
                    Notes = x.Notes,
                    Feedback = x.Service.Rates.Where(r => r.CustomerId == x.CustomerId).OrderByDescending(r => r.Id).FirstOrDefault().Feedback,
                    ServiceRequestId = x.Id,
                    Base64Image = GetServiceImageBase64(x.Service.ServiceProvider.ProfileImagePath),

                }).ToListAsync();
        }
        private async Task<List<SearchResultDto>> GetALLCustomerSideAsync(int customerId, string requestState)
        {

            return await _context.ServiceRequests.Include(x => x.Service)
     .ThenInclude(x => x.Category)
     .Include(x => x.Service)
     .ThenInclude(x => x.ServiceProvider)
     .Include(x => x.Service)
     .ThenInclude(x => x.Rates)
     .Where(x => x.RequestState == requestState && x.CustomerId == customerId)
     .Select(x => new SearchResultDto
     {
         Id = x.ServiceId,
         CategoryName = x.Service.Category.Name,
         ProfileImagePath = x.Service.ServiceProvider.ProfileImagePath,
         ServiceDescription = x.Service.Description,
         ServiceName = x.Service.Name,
         ServicePrice = x.Service.Price,
         totalRate = (x.Service.Rates.Count() == 0) ? 0 : (float)x.Service.Rates.Select(x => x.RateValue).Sum() / (float)x.Service.Rates.Count(),
         Username = x.Service.ServiceProvider.UserName,
         Notes = x.Notes,
         ServiceRequestId = x.Id
     }).ToListAsync();
        }
    }
}
