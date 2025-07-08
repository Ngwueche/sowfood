using Microsoft.EntityFrameworkCore;
using SowFoodProject.Data;

namespace SowFoodProject.Infrastructure.Utilities
{
    public class IdGenerator
    {
        private readonly ApplicationDbContext _context;
        public IdGenerator(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> GenerateStaffIdAsync(string companyId)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(companyId))
                    throw new ArgumentException("CompanyId is required", nameof(companyId));

                // Fetch the company
                var company = await _context.SowFoodCompanies
                    .Where(c => c.Id == companyId)
                    .FirstOrDefaultAsync();

                if (company == null)
                    throw new InvalidOperationException("Company not found.");

                // Generate a short company code (e.g., "SABIMARKET" → "SABI")
                var companyCode = new string(company.CompanyName
                    .Where(char.IsLetter)
                    .Take(4)
                    .ToArray())
                    .ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(companyCode))
                    companyCode = "COMP";

                // Build date segment
                string dateSegment = DateTime.UtcNow.ToString("yyyyMMdd");

                // Get last staff for the company
                var lastStaff = await _context.SowFoodCompanyStaff
                    .Where(s => s.SowFoodCompanyId == companyId && s.StaffId.StartsWith($"{companyCode}/STAFF/{dateSegment}"))
                    .OrderByDescending(s => s.StaffId)
                    .FirstOrDefaultAsync();
                if (lastStaff is null)
                    return $"{companyCode}/STAFF/0001";
                int lastNumber = 0;

                if (lastStaff != null)
                {
                    var parts = lastStaff.StaffId.Split('/');
                    if (parts.Length >= 4 && int.TryParse(parts.Last(), out var parsedNumber))
                    {
                        lastNumber = parsedNumber;
                    }
                }

                int newNumber = lastNumber + 1;

                // Generate the staff ID
                string generatedStaffId = $"{companyCode}/STAFF/{newNumber:D4}";

                return generatedStaffId;
            }
            catch (Exception ex)
            {
                // You can log the exception if needed
                // Log.Error(ex, "Error generating Staff ID");

                throw new InvalidOperationException("An error occurred while generating the Staff ID.", ex);
            }
        }

    }
}
