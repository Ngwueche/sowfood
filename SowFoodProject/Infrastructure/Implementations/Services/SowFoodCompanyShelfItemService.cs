using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;
using SowFoodProject.Data;
using SowFoodProject.Infrastructure.Utilities;
using SowFoodProject.Models.Entities.SowFoodLinkUp;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Infrastructure.Implementations.Services
{
    public class SowFoodCompanyShelfItemService : ISowFoodCompanyShelfItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SowFoodCompanyShelfItemService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse> CreateShelfItemAsync(CreateShelfItemDto dto)
        {
            try
            {
                var loggInuser = Helpers.GetUserContext(_httpContextAccessor);

                if (string.IsNullOrWhiteSpace(loggInuser.UserId))
                    return BaseApiResponse.Fail("Unauthorized", "40");

                if (string.IsNullOrWhiteSpace(dto.Name))
                    return BaseApiResponse.Fail("Item name is required", "40");

                string companyId = loggInuser.CompanyId;


                var item = new SowFoodCompanyShelfItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = dto.Name.Trim(),
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    ImageUrl = dto.ImageUrl,
                    SowFoodCompanyId = loggInuser.CompanyId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                if (!loggInuser.Roles.Contains("ShelfStaff"))
                {
                    item.AddedById = dto.ShelfStaffId;
                }
                item.AddedById = loggInuser.StaffId;
                _context.SowFoodCompanyShelfItems.Add(item);
                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(item.Id, "Shelf item created successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error occurred: {ex.Message}", "50");
            }
        }

        public async Task<ApiResponse> UpdateShelfItemAsync(UpdateShelfItemDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Id))
                    return BaseApiResponse.Fail("Shelf item ID is required", "40");

                var item = await _context.SowFoodCompanyShelfItems.FindAsync(dto.Id);
                if (item == null)
                    return BaseApiResponse.Fail("Shelf item not found", "40");

                item.Name = dto.Name?.Trim() ?? item.Name;
                item.Quantity = dto.Quantity;
                item.UnitPrice = dto.UnitPrice;
                item.ImageUrl = dto.ImageUrl;
                item.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(null, "Shelf item updated successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error.", "50");
            }
        }

        public async Task<ApiResponse> DeleteShelfItemAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BaseApiResponse.Fail("Shelf item ID is required", "40");

                var item = await _context.SowFoodCompanyShelfItems.FindAsync(id);
                if (item == null)
                    return BaseApiResponse.Fail("Shelf item not found", "40");

                _context.SowFoodCompanyShelfItems.Remove(item);
                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(null, "Shelf item deleted successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error.", "50");
            }
        }

        public async Task<ApiResponse> GetShelfItemByIdAsync(string id, string companyId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(companyId))
                    return BaseApiResponse.Fail("Shelf item ID and Company ID are required", "40");

                var item = await _context.SowFoodCompanyShelfItems
                    .Where(x => x.Id == id && x.SowFoodCompanyId == companyId)
                    .Select(x => new GetShelfItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        ImageUrl = x.ImageUrl,
                        CompanyId = x.SowFoodCompanyId,
                        AddedAt = x.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                if (item == null)
                    return BaseApiResponse.Fail("Shelf item not found", "40");

                return BaseApiResponse.Success(item, "Shelf item retrieved successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error.", "50");
            }
        }

        public async Task<ApiResponse> GetAllShelfItemsAsync(PaginationFilter filter, string companyId, string? search)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyId))
                    return BaseApiResponse.Fail("Company ID is required", "40");

                var logginUser = Helpers.GetUserContext(_httpContextAccessor);

                if (logginUser.UserId == null)
                    return BaseApiResponse.Fail("Unauthorized access.", "401");


                var query = _context.SowFoodCompanyShelfItems
                    .Where(x => x.SowFoodCompanyId == companyId);

                // If not CompanyAdmin, filter by AddedById
                if (!logginUser.Roles.Contains("CompanyAdmin") && !string.IsNullOrWhiteSpace(logginUser.StaffId))
                {
                    query = query.Where(x => x.AddedById == logginUser.StaffId);
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(x => x.Name.Contains(search));
                }

                var result = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new GetShelfItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        ImageUrl = x.ImageUrl,
                        CompanyId = x.SowFoodCompanyId,
                        AddedAt = x.CreatedAt
                    }).Paginate(filter);

                return BaseApiResponse.Success(result, "Shelf items retrieved successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail("Error retrieving shelf items.", "50");
            }
        }
    }

}
