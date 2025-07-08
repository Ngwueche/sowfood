using SowFoodProject.Application.DTOs;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface IRoleService
    {
        Task<ApiResponse> CreateRoleAsync(CreateRoleDto dto);
        Task<ApiResponse> DeactivateRoleAsync(string id, string modifiedBy);
        Task<ApiResponse> DeleteRoleAsync(string id);
        Task<ApiResponse> GetAllRolesAsync();
        Task<ApiResponse> GetRoleByIdAsync(string id);
        Task<ApiResponse> UpdateRoleAsync(UpdateRoleDto dto);
    }
}