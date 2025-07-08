using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SowFoodProject.Models.Entities
{


    [Table("AspNetRoles")]
    public class ApplicationRole : IdentityRole
    {
        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? LastModifiedAt { get; set; }

        public string? LastModifiedBy { get; set; }

        //public virtual ICollection<RolePermission> Permissions { get; set; }

        //public ApplicationRole() : base()
        //{
        //    Permissions = new HashSet<RolePermission>();
        //}

        //public ApplicationRole(string roleName) : base(roleName)
        //{
        //    Permissions = new HashSet<RolePermission>();
        //}
    }

    //[Table("RolePermissions")]
    //public class RolePermission
    //{
    //    public string Id { get; set; } = Guid.NewGuid().ToString();

    //    [Required]
    //    [MaxLength(100)]
    //    public string Name { get; set; }

    //    [Required]
    //    public bool IsGranted { get; set; }

    //    [Required]
    //    public string RoleId { get; set; }

    //    [ForeignKey("RoleId")]
    //    [DeleteBehavior(DeleteBehavior.NoAction)]
    //    public virtual ApplicationRole Role { get; set; }
    //}
}