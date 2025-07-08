using SowFoodProject.Application.DTOs;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface IDashBoardService
    {
        Task<ApiResponse> GetDashBoard();
    }
}