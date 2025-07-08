namespace SowFoodProject.Application.DTOs
{
    public class CreateStaffAppraiserDto
    {
        public string SowFoodCompanyStaffId { get; set; }
        public string? UserId { get; set; }
        public string Remark { get; set; }
    }

    public class UpdateStaffAppraiserDto
    {
        public string Id { get; set; }
        public string SowFoodCompanyStaffId { get; set; }
        public string? UserId { get; set; }
        public string Remark { get; set; }
    }

    public class GetStaffAppraiserDto
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string AppraiserId { get; set; }
        public string Remark { get; set; }
        public string AppraiserFullName { get; set; }
    }

}
