using Microsoft.EntityFrameworkCore;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;
using SowFoodProject.Data;
using SowFoodProject.Infrastructure.Utilities;
using SowFoodProject.Models.Entities.SowFoodLinkUp;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Infrastructure.Implementations.Services
{

    public class SowFoodCompanyProductionItemService : ISowFoodCompanyProductionItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SowFoodCompanyProductionItemService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse> CreateProductionItemAsync(CreateProductionItemDto dto)
        {
            try
            {
                var loggInuser = Helpers.GetUserContext(_httpContextAccessor);

                if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(loggInuser.CompanyId))
                    return BaseApiResponse.Fail("Name is required.", "40");

                var getProductionItem = _context.SowFoodCompanyProductionItems.FirstOrDefault(p => p.Name.ToLower() == dto.Name.ToLower() && p.SowFoodCompanyId == loggInuser.CompanyId);
                if (getProductionItem != null)
                {
                    return BaseApiResponse.Fail("This record already exist. Kindly update it instead.", "40");
                }

                var item = new SowFoodCompanyProductionItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = dto.Name.Trim(),
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    ImageUrl = dto.ImageUrl,
                    DateOfProduction = dto.DateOfProduction,
                    SowFoodCompanyId = loggInuser.CompanyId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                if (!loggInuser.Roles.Contains("ProductionStaff"))
                {
                    item.AddedById = dto.ProductionStaffId;
                }
                else
                {
                    item.AddedById = loggInuser.StaffId;
                }

                _context.SowFoodCompanyProductionItems.Add(item);

                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(null, getProductionItem == null ? "Production item created successfully." : "Production item updated successfully.");

            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error creating item.", "50");
            }
        }

        public async Task<ApiResponse> GetProductionItemByIdAsync(string id, string companyId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BaseApiResponse.Fail("Id is required", "40");

                var item = await _context.SowFoodCompanyProductionItems
                    .FirstOrDefaultAsync(x => x.Id == id && x.SowFoodCompanyId == companyId);

                if (item == null)
                    return BaseApiResponse.Fail("Item not found", "40");

                var result = new GetProductionItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    ImageUrl = item.ImageUrl,
                    DateOfProduction = item.DateOfProduction,
                    CompanyId = item.SowFoodCompanyId
                };

                return BaseApiResponse.Success(result, "Item retrieved successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error retrieving item.", "50");
            }
        }

        public async Task<ApiResponse> GetAllProductionItemAsync(PaginationFilter filter, string companyId, string? search)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyId))
                    return BaseApiResponse.Fail("Company ID is required", "40");

                var logginUser = Helpers.GetUserContext(_httpContextAccessor);

                if (logginUser.UserId == null)
                    return BaseApiResponse.Fail("Unauthorized access.", "401");


                var query = _context.SowFoodCompanyProductionItems
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
                    .Select(x => new GetProductionItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        ImageUrl = x.ImageUrl,
                        CompanyId = x.SowFoodCompanyId,
                        DateOfProduction = x.CreatedAt
                    }).Paginate(filter);

                return BaseApiResponse.Success(result, "Shelf items retrieved successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail("Error retrieving shelf items.", "50");
            }
        }

        public async Task<ApiResponse> UpdateProductionItemAsync(UpdateProductionItemDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Id) || string.IsNullOrWhiteSpace(dto.Name))
                    return BaseApiResponse.Fail("Id and Name are required.", "40");

                var item = await _context.SowFoodCompanyProductionItems.FirstOrDefaultAsync(x => x.Id == dto.Id && x.SowFoodCompanyId == dto.CompanyId);
                if (item == null)
                    return BaseApiResponse.Fail("Item not found", "40");

                item.Name = dto.Name.Trim();
                item.Quantity = dto.Quantity;
                item.UnitPrice = dto.UnitPrice;
                item.ImageUrl = dto.ImageUrl;
                item.DateOfProduction = dto.DateOfProduction;
                item.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(null, "Production item updated successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error updating item.", "50");
            }
        }

        public async Task<ApiResponse> DeleteProductionItemAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BaseApiResponse.Fail("Id is required.", "40");

                var item = await _context.SowFoodCompanyProductionItems.FirstOrDefaultAsync(x => x.Id == id);
                if (item == null)
                    return BaseApiResponse.Fail("Item not found", "40");

                _context.SowFoodCompanyProductionItems.Remove(item);
                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(null, "Production item deleted successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error deleting item.", "50");
            }
        }
    }

}
