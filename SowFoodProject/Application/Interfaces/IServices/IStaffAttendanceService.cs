using SowFoodProject.Application.DTOs;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface ISowFoodCompanyStaffAttendanceService
    {
        Task<ApiResponse> CreateAttendanceAsync(CreateStaffAttendanceDto dto);
        Task<ApiResponse> DeleteAttendanceAsync(string id);
        Task<ApiResponse> GetAllAttendancesAsync(PaginationFilter filter, string? staffId = null);
        Task<ApiResponse> GetAttendanceByIdAsync(string id);
        Task<ApiResponse> UpdateAttendanceAsync(UpdateStaffAttendanceDto dto);
    }
}