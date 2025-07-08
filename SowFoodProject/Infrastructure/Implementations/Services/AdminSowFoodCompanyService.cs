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
    public class AdminSowFoodCompanyService : IAdminSowFoodCompanyService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _contextAccessor;


        public AdminSowFoodCompanyService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<ApiResponse> CreateAsync(CreateSowFoodCompanyDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.CompanyName))
                    return BaseApiResponse.Fail("CompanyName is required.", "40");

                var exists = await _context.SowFoodCompanies
                    .AnyAsync(x => x.CompanyName.ToLower() == dto.CompanyName.Trim().ToLower());

                if (exists)
                    return BaseApiResponse.Fail("Company already exists.", "40");

                var company = new SowFoodCompany
                {
                    Id = Guid.NewGuid().ToString(),
                    CompanyName = dto.CompanyName.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.SowFoodCompanies.Add(company);
                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(company.Id, "Company created successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error occurred while creating company.", "50");
            }
        }
        public async Task<ApiResponse> CreateCompanyAdminAsync(CreateSowFoodCompanyStaffDto dto)
        {
            try
            {
                var loggedInUser = Helpers.GetUserDetails(_contextAccessor);
                if (!loggedInUser.Role.ToLower().Contains("admin"))
                    return BaseApiResponse.Fail("You are not authorized to perform this action.", "40");
                if (string.IsNullOrWhiteSpace(dto.CompanyId))
                    return BaseApiResponse.Fail("CompanyId is required.", "40");

                var companyExist = await _context.SowFoodCompanies
                    .FirstOrDefaultAsync(x => x.Id == dto.CompanyId);

                if (companyExist is null)
                    return BaseApiResponse.Fail("Company not found.", "40");

                if (!companyExist.IsActive)
                    return BaseApiResponse.Fail("Company is deactivated, kindly contact admin.", "40");

                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    SowFoodCompanyId = dto.CompanyId,
                    UserName = dto.EmailAddress.ToLower(),
                    Address = dto.ContactAddress,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Gender = dto.Gender,
                    ProfileImageUrl = dto.ProfileImageUrl,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.EmailAddress.ToLower(),
                    EmailConfirmed = true // Optional: skip confirmation for admin
                };

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BaseApiResponse.Fail($"User creation failed: {errors}", "40");
                }

                // Validate all roles exist
                foreach (var roleName in dto.RoleNames)
                {
                    var roleExist = await _roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                        return BaseApiResponse.Fail($"Role '{roleName}' does not exist.", "40");
                }

                var assignRoles = await _userManager.AddToRolesAsync(user, dto.RoleNames);
                if (!assignRoles.Succeeded)
                {
                    var roleErrors = string.Join(", ", assignRoles.Errors.Select(e => e.Description));
                    return BaseApiResponse.Fail($"Role assignment failed: {roleErrors}", "40");
                }

                return BaseApiResponse.Success(user.Id, "Company admin created and roles assigned successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error occurred while creating company admin.", "50");
            }
        }

        public async Task<ApiResponse> GetAllCompaniesAsync()
        {
            try
            {
                var companies = await _context.SowFoodCompanies
                    .Include(c => c.User)
                    .Select(c => new GetSowFoodCompanyDto
                    {
                        CompanyId = c.Id,
                        CompanyName = c.CompanyName,
                        IsActive = c.IsActive
                    })
                    .ToListAsync();

                return BaseApiResponse.Success(companies, "Companies retrieved successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error occurred while retrieving companies.", "50");
            }
        }

        public async Task<ApiResponse> GetByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BaseApiResponse.Fail("Company ID is required.", "40");

                var company = await _context.SowFoodCompanies
                    .Where(c => c.Id == id)
                    .Select(c => new GetSowFoodCompanyDto
                    {
                        CompanyId = c.Id,
                        CompanyName = c.CompanyName,
                        IsActive = c.IsActive
                    })
                    .FirstOrDefaultAsync();

                if (company == null)
                    return BaseApiResponse.Fail("Company not found.", "04");

                return BaseApiResponse.Success(company, "Company retrieved successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error occurred while retrieving company.", "50");
            }
        }

        public async Task<ApiResponse> UpdateAsync(UpdateSowFoodCompanyDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.CompanyId) || string.IsNullOrWhiteSpace(dto.CompanyName))
                    return BaseApiResponse.Fail("Id and CompanyName are required.", "40");

                var company = await _context.SowFoodCompanies.FirstOrDefaultAsync(c => c.Id == dto.CompanyId);
                if (company == null)
                    return BaseApiResponse.Fail("Company not found.", "40");

                company.CompanyName = dto.CompanyName.Trim();
                if (dto.IsActive != company.IsActive)
                    company.IsActive = dto.IsActive;

                company.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(null, "Company updated successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error occurred while updating company.", "50");
            }
        }

        public async Task<ApiResponse> DeactivateAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BaseApiResponse.Fail("Company ID is required.", "40");

                var company = await _context.SowFoodCompanies.FirstOrDefaultAsync(c => c.Id == id);
                if (company == null)
                    return BaseApiResponse.Fail("Company not found.", "40");

                company.IsActive = false;
                company.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return BaseApiResponse.Success(null, "Company deactivated successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error occurred while deactivating company.", "50");
            }
        }

        public async Task<ApiResponse> DeleteAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BaseApiResponse.Fail("Company ID is required.", "40");

                var company = await _context.SowFoodCompanies.FirstOrDefaultAsync(c => c.Id == id);
                if (company == null)
                    return BaseApiResponse.Fail("Company not found.", "40");

                _context.SowFoodCompanies.Remove(company);
                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(null, "Company deleted successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error occurred while deleting company.", "50");
            }
        }

    }

}
