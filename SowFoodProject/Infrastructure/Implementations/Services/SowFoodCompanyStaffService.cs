using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;
using SowFoodProject.Data;
using SowFoodProject.Infrastructure.Utilities;
using SowFoodProject.Models.Entities;
using SowFoodProject.Models.Entities.SowFoodLinkUp;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Infrastructure.Implementations.Services
{
    public class SowFoodCompanyStaffService : ISowFoodCompanyStaffService
    {
        private readonly IdGenerator _idGenerator;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public SowFoodCompanyStaffService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IdGenerator idGenerator)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _idGenerator = idGenerator;
        }


        public async Task<ApiResponse> CreateAsync(CreateSowFoodCompanyStaffDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.CompanyId))
                    return BaseApiResponse.Fail("CompanyId and StaffId are required.", "40");

                if (string.IsNullOrWhiteSpace(dto.EmailAddress) || string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
                    return BaseApiResponse.Fail("FirstName, LastName, and Email are required.", "40");

                if (await _context.SowFoodCompanyStaff
                        .AnyAsync(x => x.SowFoodCompanyId == dto.CompanyId && x.User.Email.ToLower() == dto.EmailAddress.ToLower()))
                    return BaseApiResponse.Fail("Staff already exists in the company.", "40");

                var company = await _context.SowFoodCompanies.FindAsync(dto.CompanyId);
                if (company is null || !company.IsActive)
                    return BaseApiResponse.Fail("Invalid or inactive company.", "40");

                // Use PhoneNumber as password if not provided
                var password = string.IsNullOrWhiteSpace(dto.Password)
                    ? dto.PhoneNumber ?? throw new Exception("Phone number is required when no password is supplied.")
                    : dto.Password;

                // Create application user
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    SowFoodCompanyId = dto.CompanyId,
                    UserName = dto.EmailAddress.ToLower(),
                    Email = dto.EmailAddress.ToLower(),
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                    ProfileImageUrl = dto.ProfileImageUrl,
                    EmailConfirmed = true
                };

                var userResult = await _userManager.CreateAsync(user, password);
                if (!userResult.Succeeded)
                {
                    var errors = string.Join(", ", userResult.Errors.Select(e => e.Description));
                    return BaseApiResponse.Fail($"User creation failed: {errors}", "41");
                }

                // Assign roles
                foreach (var roleName in dto.RoleNames.Distinct())
                {
                    var roleExists = await _roleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                        return BaseApiResponse.Fail($"Role '{roleName}' does not exist.", "42");

                    await _userManager.AddToRoleAsync(user, roleName);
                }
                var getStaffId = await _idGenerator.GenerateStaffIdAsync(dto.CompanyId);
                // Create staff record
                var staff = new SowFoodCompanyStaff
                {
                    Id = Guid.NewGuid().ToString(),
                    SowFoodCompanyId = dto.CompanyId,
                    StaffId = getStaffId,
                    UserId = user.Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.SowFoodCompanyStaff.Add(staff);
                await _context.SaveChangesAsync();

                var credentials = new
                {
                    StaffId = staff.StaffId,
                    Email = user.Email,
                    Password = password // Consider logging this only in dev; avoid in production logs
                };

                return BaseApiResponse.Success(credentials, "Staff and user created successfully. Login credentials returned.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error occurred while creating staff.", "50");
            }
        }

        public async Task<ApiResponse> GetAllStaffAsync(PaginationFilter filter, string companyId, string? searchString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyId))
                    return BaseApiResponse.Fail("Company ID is required", "40");

                var query = _context.SowFoodCompanyStaff
                    .Include(x => x.User)
                    .Where(x => x.SowFoodCompanyId == companyId);

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    query = query.Where(s => s.User.FirstName.Contains(searchString) ||
                                             s.User.LastName.Contains(searchString) ||
                                             s.User.Email.Contains(searchString));
                }

                var totalRecords = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize);

                var staffList = await query
                    .Select(x => new GetSowFoodCompanyStaffDto
                    {
                        StaffId = x.StaffId,
                        CompanyId = x.SowFoodCompanyId,
                        Email = x.User.Email,
                        FullName = $"{x.User.FirstName} {x.User.LastName}",
                        PhoneNumber = x.User.PhoneNumber,
                    }).Paginate(filter);


                return BaseApiResponse.Success(staffList, "Staff fetched successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error fetching staff.", "50");
            }
        }

        public async Task<ApiResponse> UpdateStaffAsync(UpdateSowFoodCompanyStaffDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.StaffId) || string.IsNullOrWhiteSpace(dto.CompanyId))
                    return BaseApiResponse.Fail("StaffId and CompanyId are required", "40");

                var staff = await _context.SowFoodCompanyStaff
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.StaffId == dto.StaffId && s.SowFoodCompanyId == dto.CompanyId);

                if (staff == null)
                    return BaseApiResponse.Fail("Staff not found", "40");


                if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                    staff.User.PhoneNumber = dto.PhoneNumber;

                staff.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(null, "Staff updated successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error updating staff.", "50");
            }
        }

        public async Task<ApiResponse> DeleteStaffAsync(string staffId, string companyId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(staffId) || string.IsNullOrWhiteSpace(companyId))
                    return BaseApiResponse.Fail("StaffId and CompanyId are required", "40");

                var staff = await _context.SowFoodCompanyStaff
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.StaffId == staffId && s.SowFoodCompanyId == companyId);

                if (staff == null)
                    return BaseApiResponse.Fail("Staff not found", "40");

                _context.SowFoodCompanyStaff.Remove(staff);
                _context.ApplicationUsers.Remove(staff.User); // optionally delete user

                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(null, "Staff deleted successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error deleting staff.", "50");
            }
        }

        public async Task<ApiResponse> GetByIdAsync(string staffId, string companyId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(staffId) || string.IsNullOrWhiteSpace(companyId))
                    return BaseApiResponse.Fail("StaffId and CompanyId are required.", "40");

                var staff = await _context.SowFoodCompanyStaff
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.StaffId == staffId && s.SowFoodCompanyId == companyId);

                if (staff == null)
                    return BaseApiResponse.Fail("Staff not found.", "04");

                var staffDto = new GetSowFoodCompanyStaffDto
                {
                    StaffId = staff.StaffId,
                    CompanyId = staff.SowFoodCompanyId,
                    FullName = $"{staff.User?.FirstName} {staff.User?.LastName}",
                    PhoneNumber = staff.User?.PhoneNumber,
                    Email = staff.User?.Email
                };

                return BaseApiResponse.Success(staffDto, "Staff retrieved successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error retrieving staff.", "50");
            }
        }

    }
}