namespace SowFoodProject.Application.DTOs
{
    public class CreateRoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
    }

    public class UpdateRoleDto
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class GetRoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

}
