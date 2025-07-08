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
    public class SowFoodCompanySalesRecordService : ISowFoodCompanySalesRecordService
    //: ISowFoodCompanySalesRecordService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public SowFoodCompanySalesRecordService(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<ApiResponse> CreateSalesRecordAsync(CreateSalesRecordDto dto)
        {
            try
            {
                if (dto.Quantity <= 0 || string.IsNullOrWhiteSpace(dto.ShelfItemId))
                    return BaseApiResponse.Fail("Invalid sales input", "40");

                var shelfItem = await _context.SowFoodCompanyShelfItems
                    .FirstOrDefaultAsync(x => x.Id == dto.ShelfItemId && x.SowFoodCompanyId == dto.CompanyId);

                if (shelfItem == null)
                    return BaseApiResponse.Fail("Shelf item not found", "40");

                if (shelfItem.Quantity < dto.Quantity)
                    return BaseApiResponse.Fail("Insufficient quantity on shelf", "40");

                var sale = new SowFoodCompanySalesRecord
                {
                    Id = Guid.NewGuid().ToString(),
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    SowFoodCompanyId = dto.CompanyId,
                    SowFoodCompanyShelfItemId = dto.ShelfItemId,
                    SowFoodCompanyCustomerId = dto.CustomerId,
                    SowFoodCompanyStaffId = dto.StaffId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                // Reduce quantity on shelf
                shelfItem.Quantity -= dto.Quantity;

                _context.SowFoodCompanySalesRecords.Add(sale);
                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(sale.Id, "Sales record created successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail("Error occurred: " + ex.Message, "50");
            }
        }

        public async Task<ApiResponse> GetAllSalesRecordAsync(PaginationFilter filter, string companyId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyId))
                    return BaseApiResponse.Fail("Company ID is required", "40");

                var logginUser = Helpers.GetUserContext(_contextAccessor);

                var query = _context.SowFoodCompanySalesRecords
                    .Include(x => x.SowFoodCompanyStaff).ThenInclude(s => s.User)
                    .Include(x => x.SowFoodCompanyShelfItem)
                    .Include(x => x.SowFoodCompanyCustomer).ThenInclude(c => c.User)
                    .Where(x => x.SowFoodCompanyId == companyId);

                // If not CompanyAdmin, restrict to sales by the logged-in staff
                if (!logginUser.Roles.Contains("CompanyAdmin") && !string.IsNullOrWhiteSpace(logginUser.StaffId))
                {
                    query = query.Where(x => x.SowFoodCompanyStaffId == logginUser.StaffId);
                }

                var records = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new GetSalesRecordDto
                    {
                        Id = x.Id,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        TotalPrice = x.TotalPrice,
                        ShelfItemName = x.SowFoodCompanyShelfItem.Name,
                        StaffName = $"{x.SowFoodCompanyStaff.User.FirstName} {x.SowFoodCompanyStaff.User.LastName}",
                        CustomerName = x.SowFoodCompanyCustomer != null
                            ? $"{x.SowFoodCompanyCustomer.User.FirstName} {x.SowFoodCompanyCustomer.User.LastName}"
                            : null,
                        Date = x.CreatedAt
                    }).Paginate(filter);

                return BaseApiResponse.Success(records, "Sales records retrieved successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail("Error retrieving sales records.", "50");
            }
        }

        public async Task<ApiResponse> GetSalesRecordByIdAsync(string id, string companyId)
        {
            try
            {
                var record = await _context.SowFoodCompanySalesRecords
                    .Include(x => x.SowFoodCompanyStaff).ThenInclude(s => s.User)
                    .Include(x => x.SowFoodCompanyShelfItem)
                    .Include(x => x.SowFoodCompanyCustomer).ThenInclude(c => c.User)
                    .FirstOrDefaultAsync(x => x.Id == id && x.SowFoodCompanyId == companyId);

                if (record == null)
                    return BaseApiResponse.Fail("Sales record not found", "40");

                var dto = new GetSalesRecordDto
                {
                    Id = record.Id,
                    Quantity = record.Quantity,
                    UnitPrice = record.UnitPrice,
                    TotalPrice = record.TotalPrice,
                    ShelfItemName = record.SowFoodCompanyShelfItem?.Name,
                    StaffName = $"{record.SowFoodCompanyStaff.User.FirstName} {record.SowFoodCompanyStaff.User.LastName}",
                    CustomerName = record.SowFoodCompanyCustomer != null ?
                        $"{record.SowFoodCompanyCustomer.User.FirstName} {record.SowFoodCompanyCustomer.User.LastName}" : null,
                    Date = record.CreatedAt
                };

                return BaseApiResponse.Success(dto, "Sales record retrieved.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error.", "50");
            }
        }

        public async Task<ApiResponse> DeleteSalesRecordAsync(string id)
        {
            try
            {
                var record = await _context.SowFoodCompanySalesRecords.FirstOrDefaultAsync(x => x.Id == id);
                if (record == null)
                    return BaseApiResponse.Fail("Record not found", "40");

                _context.SowFoodCompanySalesRecords.Remove(record);
                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(null, "Sales record deleted.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error.", "50");
            }
        }


        public async Task<ApiResponse> UpdateSalesRecordAsync(UpdateSalesRecordDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Id))
                    return BaseApiResponse.Fail("Record ID is required", "40");

                var record = await _context.SowFoodCompanySalesRecords
                    .Include(r => r.SowFoodCompanyShelfItem)
                    .FirstOrDefaultAsync(r => r.Id == dto.Id && r.SowFoodCompanyId == dto.CompanyId);

                if (record == null)
                    return BaseApiResponse.Fail("Sales record not found", "40");

                var shelfItem = await _context.SowFoodCompanyShelfItems
                    .FirstOrDefaultAsync(s => s.Id == dto.ShelfItemId && s.SowFoodCompanyId == dto.CompanyId);

                if (shelfItem == null)
                    return BaseApiResponse.Fail("Shelf item not found", "40");

                // Adjust shelf item quantity
                if (dto.ShelfItemId == record.SowFoodCompanyShelfItemId)
                {
                    // Same shelf item: update difference
                    int quantityDifference = dto.Quantity - record.Quantity;

                    if (shelfItem.Quantity < quantityDifference)
                        return BaseApiResponse.Fail("Not enough quantity available on shelf to update.", "40");

                    shelfItem.Quantity -= quantityDifference;
                }
                else
                {
                    // Revert quantity to old shelf item
                    var oldShelfItem = await _context.SowFoodCompanyShelfItems
                        .FirstOrDefaultAsync(s => s.Id == record.SowFoodCompanyShelfItemId);

                    if (oldShelfItem != null)
                        oldShelfItem.Quantity += record.Quantity;

                    // Deduct from new shelf item
                    if (shelfItem.Quantity < dto.Quantity)
                        return BaseApiResponse.Fail("Not enough quantity available on new shelf item.", "40");

                    shelfItem.Quantity -= dto.Quantity;
                    record.SowFoodCompanyShelfItemId = dto.ShelfItemId;
                }

                // Update sales record
                record.Quantity = dto.Quantity;
                record.UnitPrice = dto.UnitPrice;
                record.SowFoodCompanyCustomerId = dto.CustomerId;
                record.SowFoodCompanyStaffId = dto.StaffId;
                record.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return BaseApiResponse.Success(null, "Sales record updated successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail("Error updating sales record: " + ex.Message, "50");
            }
        }

    }
}