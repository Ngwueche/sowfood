using Microsoft.AspNetCore.Identity;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;
using SowFoodProject.Models.Entities;
using static SowFoodProject.Application.DTOs.BaseApiResponse;

namespace SowFoodProject.Infrastructure.Implementations.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApiResponse> CreateRoleAsync(CreateRoleDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    return BaseApiResponse.Fail("Role name is required", "40");

                var exists = await _roleManager.RoleExistsAsync(dto.Name);
                if (exists)
                    return BaseApiResponse.Fail("Role already exists", "40");

                var role = new ApplicationRole
                {
                    Name = dto.Name.Trim(),
                    NormalizedName = dto.Name.Trim().ToUpper(),
                    Description = dto.Description?.Trim(),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = dto.CreatedBy
                };

                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BaseApiResponse.Fail($"Role creation failed: {errors}", "40");
                }

                return BaseApiResponse.Success(role.Id, "Role created successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error creating role.", "50");
            }
        }

        public async Task<ApiResponse> GetAllRolesAsync()
        {
            try
            {
                var roles = _roleManager.Roles.Select(r => new GetRoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    IsActive = r.IsActive
                }).ToList();

                return BaseApiResponse.Success(roles, "Roles retrieved successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error retrieving roles.", "50");
            }
        }

        public async Task<ApiResponse> GetRoleByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BaseApiResponse.Fail("Role ID is required", "40");

                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                    return BaseApiResponse.Fail("Role not found", "04");

                var dto = new GetRoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    IsActive = role.IsActive
                };

                return BaseApiResponse.Success(dto, "Role retrieved successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error retrieving role.", "50");
            }
        }

        public async Task<ApiResponse> UpdateRoleAsync(UpdateRoleDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.RoleId) || string.IsNullOrWhiteSpace(dto.Name))
                    return BaseApiResponse.Fail("RoleId and Name are required", "40");

                var role = await _roleManager.FindByIdAsync(dto.RoleId);
                if (role == null)
                    return BaseApiResponse.Fail("Role not found", "04");

                role.Name = dto.Name.Trim();
                role.NormalizedName = dto.Name.Trim().ToUpper();
                role.Description = dto.Description?.Trim();

                var result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BaseApiResponse.Fail($"Role update failed: {errors}", "40");
                }

                return BaseApiResponse.Success(null, "Role updated successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error updating role.", "50");
            }
        }

        public async Task<ApiResponse> DeactivateRoleAsync(string id, string modifiedBy)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                    return BaseApiResponse.Fail("Role not found", "04");

                role.IsActive = false;
                role.LastModifiedAt = DateTime.UtcNow;
                role.LastModifiedBy = modifiedBy;

                await _roleManager.UpdateAsync(role);

                return BaseApiResponse.Success(null, "Role deactivated successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error deactivating role.", "50");
            }
        }

        public async Task<ApiResponse> DeleteRoleAsync(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                    return BaseApiResponse.Fail("Role not found", "04");

                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BaseApiResponse.Fail($"Role deletion failed: {errors}", "40");
                }

                return BaseApiResponse.Success(null, "Role deleted successfully.");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error deleting role.", "50");
            }
        }
    }

}
