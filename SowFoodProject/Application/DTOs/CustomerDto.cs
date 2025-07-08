namespace SowFoodProject.Application.DTOs
{
    public class CreateSowFoodCompanyCustomerDto
    {
        public string SowFoodCompanyId { get; set; }
        public string Email { get; set; }  // Will be used to create ApplicationUser
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string RegisteredBy { get; set; }
    }

    public class GetSowFoodCompanyCustomerDto
    {
        public string CustomerId { get; set; }
        public string CompanyId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateRegistered { get; set; }
    }

    public class UpdateSowFoodCompanyCustomerDto
    {
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

}
