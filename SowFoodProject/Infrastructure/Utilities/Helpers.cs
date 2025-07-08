using System.Security.Claims;

namespace SowFoodProject.Infrastructure.Utilities
{
    public static class Helpers
    {
        public static (string Id, string Role, string Email) GetUserDetails(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext.User;
            var Id = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value?.ToLower();
            var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value?.ToLower();
            var email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value?.ToLower();

            return (Id, role, email);
        }

        public static (string? UserId, List<string> Roles, string? CompanyId, string? StaffId) GetUserContext(IHttpContextAccessor httpContext)
        {
            if (httpContext == null || httpContext.HttpContext.User == null)
                return (null, new List<string>(), null, null);

            var user = httpContext.HttpContext.User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var staffId = user.FindFirst("StaffId")?.Value;
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "roles").Select(c => c.Value).ToList();
            var companyId = user.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;

            return (userId, roles, companyId, staffId);
        }
    }

}
