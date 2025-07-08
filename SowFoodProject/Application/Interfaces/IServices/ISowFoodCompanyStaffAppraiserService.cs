using SowFoodProject.Application.DTOs;

namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface ISowFoodCompanyStaffAppraiserService
    {
        Task<BaseApiResponse.ApiResponse> CreateAppraiserAsync(CreateStaffAppraiserDto dto);
        Task<BaseApiResponse.ApiResponse> DeleteAppraiserAsync(string id);
        Task<BaseApiResponse.ApiResponse> GetAllAppraisersAsync(PaginationFilter filter, string staffId, string? search);
        Task<BaseApiResponse.ApiResponse> GetAppraiserByIdAsync(string id);
        Task<BaseApiResponse.ApiResponse> UpdateAppraiserAsync(UpdateStaffAppraiserDto dto);
    }
}