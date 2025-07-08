using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;
using SowFoodProject.Data;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Infrastructure.Implementations.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DashBoardService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse> GetDashBoard()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var userId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return BaseApiResponse.Fail("Unauthorized access.", "40");

                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null || string.IsNullOrWhiteSpace(user.SowFoodCompanyId))
                    return BaseApiResponse.Fail("User does not belong to any company.", "40");

                var companyId = user.SowFoodCompanyId;

                var roles = httpContext.User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();

                // pick only one role for switch — you can adjust if multiple roles logic is needed
                var role = roles.FirstOrDefault();

                switch (role)
                {
                    case "ShelfStaff":
                        {
                            var staff = await _context.SowFoodCompanyStaff.FirstOrDefaultAsync(x => x.UserId == userId);
                            if (staff == null)
                                return BaseApiResponse.Fail("Staff record not found.", "40");

                            var customerCount = await _context.SowFoodCompanyCustomers
                                .CountAsync(c => c.SowFoodCompanyId == companyId && c.RegisteredBy == staff.StaffId);

                            var totalSalesAmount = await _context.SowFoodCompanySalesRecords
                                .Where(s => s.SowFoodCompanyId == companyId && s.SowFoodCompanyStaffId == staff.StaffId)
                                .SumAsync(s => (decimal?)s.TotalPrice) ?? 0;

                            var totalShelfItems = await _context.SowFoodCompanyShelfItems
                                .CountAsync(s => s.SowFoodCompanyId == companyId && s.AddedById == staff.StaffId);

                            return BaseApiResponse.Success(new
                            {
                                TotalCustomers = customerCount,
                                TotalSalesAmount = totalSalesAmount,
                                TotalShelfItems = totalShelfItems
                            }, "ShelfStaff summary retrieved successfully.");
                        }

                    case "ProductionStaff":
                        {
                            var staff = await _context.SowFoodCompanyStaff.FirstOrDefaultAsync(x => x.UserId == userId);
                            if (staff == null)
                                return BaseApiResponse.Fail("Staff record not found.", "40");

                            var daysAtWork = await _context.SowFoodCompanyStaffAttendances
                                .CountAsync(a => a.SowFoodCompanyStaffId == staff.StaffId);

                            var productionItems = await _context.SowFoodCompanyProductionItems
                                .CountAsync(p => p.SowFoodCompanyId == companyId && p.AddedById == staff.StaffId);

                            return BaseApiResponse.Success(new
                            {
                                DaysAtWork = daysAtWork,
                                TotalProductionItems = productionItems
                            }, "ProductionStaff summary retrieved successfully.");
                        }

                    case "Shelf":
                        {
                            var staff = await _context.SowFoodCompanyStaff.FirstOrDefaultAsync(x => x.UserId == userId);
                            if (staff == null)
                                return BaseApiResponse.Fail("Staff record not found.", "40");

                            var daysAtWork = await _context.SowFoodCompanyStaffAttendances
                                .CountAsync(a => a.SowFoodCompanyStaffId == staff.StaffId);

                            var shelfItems = await _context.SowFoodCompanyShelfItems
                                .CountAsync(s => s.SowFoodCompanyId == companyId && s.AddedById == staff.StaffId);

                            return BaseApiResponse.Success(new
                            {
                                DaysAtWork = daysAtWork,
                                TotalShelfItems = shelfItems
                            }, "Shelf summary retrieved successfully.");
                        }

                    case "CompanyAdmin":
                    default:
                        {
                            var totalStaff = await _context.SowFoodCompanyStaff
                                .CountAsync(s => s.SowFoodCompanyId == companyId);

                            var totalProductionItems = await _context.SowFoodCompanyProductionItems
                                .CountAsync(p => p.SowFoodCompanyId == companyId);

                            var totalSalesAmount = await _context.SowFoodCompanySalesRecords
                                .Where(s => s.SowFoodCompanyId == companyId)
                                .SumAsync(s => (decimal?)s.TotalPrice) ?? 0;

                            var stats = new CompanySummaryStatsDto
                            {
                                TotalStaff = totalStaff,
                                TotalProductionItems = totalProductionItems,
                                TotalSalesAmount = totalSalesAmount
                            };

                            return BaseApiResponse.Success(stats, "Company summary retrieved successfully.");
                        }
                }
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error retrieving summary: {ex.Message}", "50");
            }
        }


    }
}
