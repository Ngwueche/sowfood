using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SowFoodProject.Models.Entities;

namespace SowFoodProject.Models.Entities.SowFoodLinkUp
{
    [Table("SowFoodCompanyCustomers")]
    public class SowFoodCompanyCustomer : BaseEntity
    {
        [Required]
        public string SowFoodCompanyId { get; set; }

        public string? UserId { get; set; }

        [Required]
        public string RegisteredBy { get; set; }

        [ForeignKey("UserId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("SowFoodCompanyId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual SowFoodCompany SowFoodCompany { get; set; }

        public virtual ICollection<SowFoodCompanySalesRecord> SowFoodCompanySalesRecords { get; set; } = new List<SowFoodCompanySalesRecord>();
    }
}