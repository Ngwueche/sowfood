using SowFoodProject.Application.DTOs;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface ISowFoodCompanyProductionItemService
    {
        Task<ApiResponse> CreateProductionItemAsync(CreateProductionItemDto dto);
        Task<ApiResponse> DeleteProductionItemAsync(string id);
        Task<ApiResponse> GetAllProductionItemAsync(PaginationFilter filter, string companyId, string? search);
        Task<ApiResponse> GetProductionItemByIdAsync(string id, string companyId);
        Task<ApiResponse> UpdateProductionItemAsync(UpdateProductionItemDto dto);
    }
}