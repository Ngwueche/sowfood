using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;
using SowFoodProject.Data;
using SowFoodProject.Infrastructure.Utilities;
using SowFoodProject.Models.Entities;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Infrastructure.Implementations.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config,
                           IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _userManager = userManager;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<ApiResponse> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || user.IsBlocked)
                return BaseApiResponse.Fail("Invalid login credentials", "40");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return BaseApiResponse.Fail("Invalid login credentials", "40");

            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var staff = await _context.SowFoodCompanyStaff.FirstOrDefaultAsync(x => x.UserId == user.Id);
            var roles = (await _userManager.GetRolesAsync(user)).ToList();

            var token = GenerateJwtToken(user, roles, staff?.StaffId);
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            return BaseApiResponse.Success(new AuthResponseDto
            {
                Token = token,
                RefreshToken = user.RefreshToken,
                Expiry = DateTime.UtcNow.AddMinutes(60)
            }, "Login successful");
        }

        public async Task<ApiResponse> RefreshTokenAsync(string refreshToken)
        {
            var user = _context.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return BaseApiResponse.Fail("Invalid or expired refresh token", "40");

            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            var staff = await _context.SowFoodCompanyStaff.FirstOrDefaultAsync(x => x.UserId == user.Id);

            var newToken = GenerateJwtToken(user, roles, staff?.StaffId);
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return BaseApiResponse.Success(new AuthResponseDto
            {
                Token = newToken,
                RefreshToken = user.RefreshToken,
                Expiry = DateTime.UtcNow.AddMinutes(60)
            }, "Token refreshed successfully");
        }

        public async Task<ApiResponse> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return BaseApiResponse.Fail("User not found", "40");

            user.PasswordResetToken = Guid.NewGuid().ToString();
            user.PasswordResetExpiry = DateTime.UtcNow.AddMinutes(15);
            await _context.SaveChangesAsync();

            // You would send this token by email in real scenario
            return BaseApiResponse.Success(user.PasswordResetToken, "Password reset token generated");
        }

        public async Task<ApiResponse> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return BaseApiResponse.Fail("User not found", "40");

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                return BaseApiResponse.Fail("Password change failed", "40");

            return BaseApiResponse.Success(null, "Password changed successfully");
        }

        private string GenerateJwtToken(ApplicationUser user, List<string> roles, string staffId)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("StaffId", staffId ?? string.Empty)
        };

            if (!string.IsNullOrWhiteSpace(user.SowFoodCompanyId))
                claims.Add(new Claim("CompanyId", user.SowFoodCompanyId));

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(60);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
