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
    public class SowFoodCompanyStaffAttendanceService : ISowFoodCompanyStaffAttendanceService
    {
        private readonly ApplicationDbContext _context;

        public SowFoodCompanyStaffAttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse> CreateAttendanceAsync(CreateStaffAttendanceDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.StaffId))
                    return BaseApiResponse.Fail("StaffId is required", "40");

                var staff = await _context.SowFoodCompanyStaff.FirstOrDefaultAsync(x => x.Id == dto.StaffId);
                if (staff == null)
                    return BaseApiResponse.Fail("Staff not found", "40");

                var attendance = new SowFoodCompanyStaffAttendance
                {
                    Id = Guid.NewGuid().ToString(),
                    SowFoodCompanyStaffId = dto.StaffId,
                    LogonTime = dto.LogonTime,
                    LogoutTime = dto.LogoutTime,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.SowFoodCompanyStaffAttendances.AddAsync(attendance);
                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(attendance.Id, "Attendance logged successfully");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error logging attendance.", "50");
            }
        }

        public async Task<ApiResponse> UpdateAttendanceAsync(UpdateStaffAttendanceDto dto)
        {
            try
            {
                var attendance = await _context.SowFoodCompanyStaffAttendances
                    .FirstOrDefaultAsync(x => x.Id == dto.AttendanceId);

                if (attendance == null)
                    return BaseApiResponse.Fail("Attendance record not found", "40");

                attendance.ConfirmedTimeIn = dto.ConfirmedTimeIn;
                attendance.IsConfirmed = dto.IsConfirmed;
                attendance.ConfirmedByUserId = dto.ConfirmedByUserId;
                attendance.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return BaseApiResponse.Success(null, "Attendance updated successfully");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error updating attendance.", "50");
            }
        }

        public async Task<ApiResponse> DeleteAttendanceAsync(string id)
        {
            try
            {
                var attendance = await _context.SowFoodCompanyStaffAttendances.FirstOrDefaultAsync(x => x.Id == id);
                if (attendance == null)
                    return BaseApiResponse.Fail("Attendance not found", "40");

                _context.SowFoodCompanyStaffAttendances.Remove(attendance);
                await _context.SaveChangesAsync();
                return BaseApiResponse.Success(null, "Attendance deleted successfully");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error deleting attendance.", "50");
            }
        }

        public async Task<ApiResponse> GetAttendanceByIdAsync(string id)
        {
            try
            {
                var record = await _context.SowFoodCompanyStaffAttendances
                    .Include(a => a.SowFoodCompanyStaff).ThenInclude(s => s.User)
                    .Include(a => a.ConfirmedByUser)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (record == null)
                    return BaseApiResponse.Fail("Attendance not found", "40");

                var dto = new GetStaffAttendanceDto
                {
                    AttendanceId = record.Id,
                    StaffId = record.SowFoodCompanyStaffId,
                    StaffName = $"{record.SowFoodCompanyStaff.User.FirstName} {record.SowFoodCompanyStaff.User.LastName}",
                    LogonTime = record.LogonTime,
                    LogoutTime = record.LogoutTime,
                    ConfirmedTimeIn = record.ConfirmedTimeIn,
                    IsConfirmed = record.IsConfirmed,
                    ConfirmedBy = record.ConfirmedByUser?.FirstName + " " + record.ConfirmedByUser?.LastName,
                    DateLogged = record.CreatedAt
                };

                return BaseApiResponse.Success(dto, "Attendance fetched successfully");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error fetching attendance.", "50");
            }
        }

        public async Task<ApiResponse> GetAllAttendancesAsync(PaginationFilter filter, string? staffId = null)
        {
            try
            {
                var query = _context.SowFoodCompanyStaffAttendances
                    .Include(x => x.SowFoodCompanyStaff).ThenInclude(u => u.User)
                    .Include(x => x.ConfirmedByUser)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(staffId))
                    query = query.Where(x => x.SowFoodCompanyStaffId == staffId);

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);

                var paged = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new GetStaffAttendanceDto
                    {
                        AttendanceId = x.Id,
                        StaffId = x.SowFoodCompanyStaffId,
                        StaffName = $"{x.SowFoodCompanyStaff.User.FirstName} {x.SowFoodCompanyStaff.User.LastName}",
                        LogonTime = x.LogonTime,
                        LogoutTime = x.LogoutTime,
                        ConfirmedTimeIn = x.ConfirmedTimeIn,
                        IsConfirmed = x.IsConfirmed,
                        ConfirmedBy = x.ConfirmedByUser != null ? $"{x.ConfirmedByUser.FirstName} {x.ConfirmedByUser.LastName}" : null,
                        DateLogged = x.CreatedAt
                    }).Paginate(filter);


                return BaseApiResponse.Success(paged, "Attendances retrieved successfully");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error fetching attendance list.", "50");
            }
        }
    }
}
