using System;
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
    public class SowFoodCompanyCustomerService : ISowFoodCompanyCustomerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public SowFoodCompanyCustomerService(ApplicationDbContext context, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApiResponse> CreateCustomerAsync(CreateSowFoodCompanyCustomerDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.SowFoodCompanyId))
                    return BaseApiResponse.Fail("Email and Company ID are required", "40");

                var exists = await _context.ApplicationUsers.AnyAsync(u => u.Email == dto.Email);
                if (exists)
                    return BaseApiResponse.Fail("A user with this email already exists", "40");

                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = dto.Email.ToLower(),
                    UserName = dto.Email.ToLower(),
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                };
                user.PasswordHash = _passwordHasher.HashPassword(user, dto.PhoneNumber ?? "DefaultPassword@123");

                await _context.ApplicationUsers.AddAsync(user);

                var customer = new SowFoodCompanyCustomer
                {
                    Id = Guid.NewGuid().ToString(),
                    SowFoodCompanyId = dto.SowFoodCompanyId,
                    RegisteredBy = dto.RegisteredBy,
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await _context.SowFoodCompanyCustomers.AddAsync(customer);
                await _context.SaveChangesAsync();

                var loginInfo = new
                {
                    Email = user.Email,
                    Password = dto.PhoneNumber ?? "DefaultPassword@123"
                };

                return BaseApiResponse.Success(loginInfo, "Customer created successfully");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error creating customer.", "50");
            }
        }

        public async Task<ApiResponse> UpdateCustomerAsync(UpdateSowFoodCompanyCustomerDto dto)
        {
            try
            {
                var customer = await _context.SowFoodCompanyCustomers
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == dto.CustomerId);

                if (customer == null)
                    return BaseApiResponse.Fail("Customer not found", "40");

                customer.User.FirstName = dto.FirstName;
                customer.User.LastName = dto.LastName;
                customer.User.PhoneNumber = dto.PhoneNumber;
                customer.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return BaseApiResponse.Success(null, "Customer updated successfully");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error updating customer.", "50");
            }
        }

        public async Task<ApiResponse> DeleteCustomerAsync(string customerId)
        {
            try
            {
                var customer = await _context.SowFoodCompanyCustomers
                    .FirstOrDefaultAsync(x => x.Id == customerId);

                if (customer == null)
                    return BaseApiResponse.Fail("Customer not found", "40");

                _context.SowFoodCompanyCustomers.Remove(customer);
                await _context.SaveChangesAsync();

                return BaseApiResponse.Success(null, "Customer deleted successfully");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error deleting customer.", "50");
            }
        }

        public async Task<ApiResponse> GetAllCustomersAsync(PaginationFilter filter, string companyId, string? search = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyId))
                    return BaseApiResponse.Fail("Company ID is required", "40");

                var query = _context.SowFoodCompanyCustomers
                    .Include(x => x.User)
                    .Where(x => x.SowFoodCompanyId == companyId);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(x =>
                        x.User.FirstName.Contains(search) ||
                        x.User.LastName.Contains(search) ||
                        x.User.Email.Contains(search));
                }

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);

                var result = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new GetSowFoodCompanyCustomerDto
                    {
                        CustomerId = x.Id,
                        CompanyId = x.SowFoodCompanyId,
                        Email = x.User.Email,
                        PhoneNumber = x.User.PhoneNumber,
                        FullName = $"{x.User.FirstName} {x.User.LastName}",
                        DateRegistered = x.CreatedAt
                    }).Paginate(filter);

                return BaseApiResponse.Success(result, "Customers retrieved successfully");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error fetching customers.", "50");
            }
        }

        public async Task<ApiResponse> GetCustomerByIdAsync(string customerId, string companyId)
        {
            try
            {
                var customer = await _context.SowFoodCompanyCustomers
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == customerId && x.SowFoodCompanyId == companyId);

                if (customer == null)
                    return BaseApiResponse.Fail("Customer not found", "40");

                var dto = new GetSowFoodCompanyCustomerDto
                {
                    CustomerId = customer.Id,
                    CompanyId = customer.SowFoodCompanyId,
                    Email = customer.User.Email,
                    PhoneNumber = customer.User.PhoneNumber,
                    FullName = $"{customer.User.FirstName} {customer.User.LastName}",
                    DateRegistered = customer.CreatedAt
                };

                return BaseApiResponse.Success(dto, "Customer retrieved successfully");
            }
            catch (Exception ex)
            {
                return BaseApiResponse.Fail($"Error retrieving customer.", "50");
            }
        }
    }

}
