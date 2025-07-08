using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SowFoodProject.Models.Entities;

namespace SowFoodProject.Models.Entities.SowFoodLinkUp
{
    [Table("SowFoodCompanyStaff")]
    public class SowFoodCompanyStaff : BaseEntity
    {
        [Required]
        public string SowFoodCompanyId { get; set; }

        public string? UserId { get; set; }

        [Required]
        public string StaffId { get; set; }

        [ForeignKey("UserId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("SowFoodCompanyId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual SowFoodCompany SowFoodCompany { get; set; }

        public virtual ICollection<SowFoodCompanyStaffAttendance> SowFoodCompanyStaffAttendances { get; set; } = new List<SowFoodCompanyStaffAttendance>();
        public virtual ICollection<SowFoodCompanySalesRecord> SowFoodCompanySalesRecords { get; set; } = new List<SowFoodCompanySalesRecord>();
    }
}