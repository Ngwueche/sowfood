using SowFoodProject.Application.DTOs;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface ISowFoodCompanyStaffService
    {
        Task<ApiResponse> CreateAsync(CreateSowFoodCompanyStaffDto dto);
        Task<ApiResponse> DeleteStaffAsync(string staffId, string companyId);
        Task<ApiResponse> GetAllStaffAsync(PaginationFilter filter, string companyId, string? searchString);
        Task<ApiResponse> GetByIdAsync(string staffId, string companyId);
        Task<ApiResponse> UpdateStaffAsync(UpdateSowFoodCompanyStaffDto dto);
    }
}