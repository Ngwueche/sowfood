using SowFoodProject.Application.DTOs;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface ISowFoodCompanySalesRecordService
    {
        Task<ApiResponse> CreateSalesRecordAsync(CreateSalesRecordDto dto);
        Task<ApiResponse> DeleteSalesRecordAsync(string id);
        Task<ApiResponse> GetAllSalesRecordAsync(PaginationFilter filter, string companyId);
        Task<ApiResponse> GetSalesRecordByIdAsync(string id, string companyId);
        Task<ApiResponse> UpdateSalesRecordAsync(UpdateSalesRecordDto dto);
    }
}