using SowFoodProject.Application.DTOs;

namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface ISowFoodCompanyShelfItemService
    {
        Task<BaseApiResponse.ApiResponse> CreateShelfItemAsync(CreateShelfItemDto dto);
        Task<BaseApiResponse.ApiResponse> DeleteShelfItemAsync(string id);
        Task<BaseApiResponse.ApiResponse> GetAllShelfItemsAsync(PaginationFilter filter, string companyId, string? search);
        Task<BaseApiResponse.ApiResponse> GetShelfItemByIdAsync(string id, string companyId);
        Task<BaseApiResponse.ApiResponse> UpdateShelfItemAsync(UpdateShelfItemDto dto);
    }
}