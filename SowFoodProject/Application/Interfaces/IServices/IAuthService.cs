using SowFoodProject.Application.DTOs;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<ApiResponse> LoginAsync(LoginDto dto);
        Task<ApiResponse> RefreshTokenAsync(string refreshToken);
        Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<ApiResponse> ChangePasswordAsync(ChangePasswordDto dto);
    }
}