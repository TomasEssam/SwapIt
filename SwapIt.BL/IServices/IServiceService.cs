using Microsoft.AspNetCore.Http;
using SwapIt.BL.DTOs;

namespace SwapIt.BL.IServices
{
    public interface IServiceService
    {
        Task<bool> CreateAsync(ServiceDto dto);
        Task<bool> DeleteAsync(int serviceId);
        Task<bool> UpdateAsync(ServiceDto dto);
        Task<List<SearchResultDto>> GetAllAcceptedServiceProviderSideAsync(int serviceProviderId);
        Task<List<SearchResultDto>> GetAllFinshiedServiceProviderSideAsync(int serviceProviderId);
        Task<List<SearchResultDto>> GetAllPendingServiceProviderSideAsync(int serviceProviderId);
        Task<List<SearchResultDto>> GetAllCanceledServiceProviderSideAsync(int serviceProviderId);

        Task<List<SearchResultDto>> GetAllAcceptedCustomerSideAsync(int customerId);
        Task<List<SearchResultDto>> GetAllFinishiedCustomerSideAsync(int customerId);
        Task<List<SearchResultDto>> GetAllPendingCustomerSideAsync(int customerId);
        Task<List<SearchResultDto>> GetAllCanceledCustomerSideAsync(int customerId);

        Task<List<ServiceDto>> GetAllAsync();
        Task<List<SearchResultDto>> SearchServiceAsync(ServiceFilterDto Filter);
        Task<List<ImageResultDto>> GetAllPreviousWorkImageUrlAsync(int serviceProviderId);
        Task<ServiceDto> GetServiceByIdAsync(int serviceId);
        Task<List<DropDownDto>> GetServiceDDAsync();
        Task<List<RateDto>> GetRatesAsync(int serviceId);
        Task<bool> UploadServiceImage(IFormFile serviceImage, int serviceId, string folderName);
        Task<ImageResultDto> GetServiceImage(int serviceId);

    }
}
