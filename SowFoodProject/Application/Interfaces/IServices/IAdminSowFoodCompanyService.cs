using SowFoodProject.Application.DTOs;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface IAdminSowFoodCompanyService
    {
        Task<ApiResponse> CreateAsync(CreateSowFoodCompanyDto dto);
        Task<ApiResponse> CreateCompanyAdminAsync(CreateSowFoodCompanyStaffDto dto);
        Task<ApiResponse> DeactivateAsync(string id);
        Task<ApiResponse> DeleteAsync(string id);
        Task<ApiResponse> GetAllCompaniesAsync();
        Task<ApiResponse> GetByIdAsync(string id);
        Task<ApiResponse> UpdateAsync(UpdateSowFoodCompanyDto dto);
    }
}