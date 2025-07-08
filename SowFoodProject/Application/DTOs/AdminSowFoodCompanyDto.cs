namespace SowFoodProject.Application.DTOs
{
    public class CreateSowFoodCompanyDto
    {
        public string CompanyName { get; set; }
    }
    public class GetSowFoodCompanyDto
    {
        public string CompanyName { get; set; }
        public string CompanyId { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateSowFoodCompanyDto
    {
        public string CompanyName { get; set; }
        public string CompanyId { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateSowFoodCompanyStaffDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactAddress { get; set; }
        public string Gender { get; set; }
        public string ProfileImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyId { get; set; }
        public string? Password { get; set; }
        public List<string>? RoleNames { get; set; }
    }


    public class UpdateSowFoodCompanyStaffDto
    {

        public string StaffId { get; set; }
        public string CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactAddress { get; set; }
        public string? ImageUrl { get; set; }
        public List<string>? RoleNames { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class GetSowFoodCompanyStaffDto
    {
        public string StaffId { get; set; }
        public string CompanyId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
