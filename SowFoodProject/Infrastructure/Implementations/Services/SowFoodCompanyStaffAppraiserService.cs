using System;
using Microsoft.EntityFrameworkCore;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;
using SowFoodProject.Data;
using SowFoodProject.Infrastructure.Utilities;
using SowFoodProject.Models.Entities.SowFoodLinkUp;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Infrastructure.Implementations.Services
{
    public class SowFoodCompanyStaffAppraiserService : ISowFoodCompanyStaffAppraiserService
    {
        private readonly ApplicationDbContext _context;

        public SowFoodCompanyStaffAppraiserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse> CreateAppraiserAsync(CreateStaffAppraiserDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.SowFoodCompanyStaffId) || string.IsNullOrWhiteSpace(dto.Remark))
                    return Fail("Staff ID and Remark are required", "40");

                var entity = new SowFoodCompanyStaffAppraiser
                {
                    Id = Guid.NewGuid().ToString(),
                    SowFoodCompanyStaffId = dto.SowFoodCompanyStaffId,
                    UserId = dto.UserId,
                    Remark = dto.Remark.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.SowFoodCompanyStaffAppraisers.Add(entity);
                await _context.SaveChangesAsync();

                return Success(entity.Id, "Appraiser created successfully.");
            }
            catch (Exception ex)
            {
                return Fail($"Error.", "50");
            }
        }

        public async Task<ApiResponse> UpdateAppraiserAsync(UpdateStaffAppraiserDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Id))
                    return Fail("Appraiser ID is required", "40");

                var appraiser = await _context.SowFoodCompanyStaffAppraisers.FindAsync(dto.Id);
                if (appraiser == null)
                    return Fail("Appraiser not found", "40");

                appraiser.SowFoodCompanyStaffId = dto.SowFoodCompanyStaffId;
                appraiser.UserId = dto.UserId;
                appraiser.Remark = dto.Remark.Trim();
                appraiser.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Success(null, "Appraiser updated successfully.");
            }
            catch (Exception ex)
            {
                return Fail($"Error.", "50");
            }
        }

        public async Task<ApiResponse> DeleteAppraiserAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return Fail("Appraiser ID is required", "40");

                var appraiser = await _context.SowFoodCompanyStaffAppraisers.FindAsync(id);
                if (appraiser == null)
                    return Fail("Appraiser not found", "40");

                _context.SowFoodCompanyStaffAppraisers.Remove(appraiser);
                await _context.SaveChangesAsync();

                return Success(null, "Appraiser deleted successfully.");
            }
            catch (Exception ex)
            {
                return Fail($"Error.", "50");
            }
        }

        public async Task<ApiResponse> GetAppraiserByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return Fail("Appraiser ID is required", "40");

                var appraiser = await _context.SowFoodCompanyStaffAppraisers
                    .Include(x => x.User)
                    .Where(x => x.Id == id)
                    .Select(x => new GetStaffAppraiserDto
                    {
                        Id = x.Id,
                        StaffId = x.SowFoodCompanyStaffId,
                        AppraiserId = x.UserId,
                        Remark = x.Remark,
                        AppraiserFullName = x.User != null ? $"{x.User.FirstName} {x.User.LastName}" : "N/A"
                    }).FirstOrDefaultAsync();

                if (appraiser == null)
                    return Fail("Appraiser not found", "40");

                return Success(appraiser, "Appraiser retrieved successfully.");
            }
            catch (Exception ex)
            {
                return Fail($"Error.", "50");
            }
        }

        public async Task<ApiResponse> GetAllAppraisersAsync(PaginationFilter filter, string staffId, string? search)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(staffId))
                    return Fail("Staff ID is required", "40");

                var query = _context.SowFoodCompanyStaffAppraisers
                    .Include(x => x.User)
                    .Where(x => x.SowFoodCompanyStaffId == staffId);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(x =>
                        x.User.FirstName.Contains(search) ||
                        x.User.LastName.Contains(search) ||
                        x.Remark.Contains(search));
                }

                var result = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new GetStaffAppraiserDto
                    {
                        Id = x.Id,
                        StaffId = x.SowFoodCompanyStaffId,
                        AppraiserId = x.UserId,
                        Remark = x.Remark,
                        AppraiserFullName = x.User != null ? $"{x.User.FirstName} {x.User.LastName}" : "N/A"
                    }).Paginate(filter);

                return Success(result, "Appraisers retrieved successfully.");
            }
            catch (Exception ex)
            {
                return Fail($"Error.", "50");
            }
        }
    }

}
