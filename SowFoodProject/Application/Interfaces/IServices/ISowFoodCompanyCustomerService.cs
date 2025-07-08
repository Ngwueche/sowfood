using SowFoodProject.Application.DTOs;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface ISowFoodCompanyCustomerService
    {
        Task<ApiResponse> CreateCustomerAsync(CreateSowFoodCompanyCustomerDto dto);
        Task<ApiResponse> DeleteCustomerAsync(string customerId);
        Task<ApiResponse> GetAllCustomersAsync(PaginationFilter filter, string companyId, string? search = null);
        Task<ApiResponse> GetCustomerByIdAsync(string customerId, string companyId);
        Task<ApiResponse> UpdateCustomerAsync(UpdateSowFoodCompanyCustomerDto dto);
    }
}