using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SowFoodProject.Models.Entities;

namespace SowFoodProject.Models.Entities.SowFoodLinkUp
{
    [Table("SowFoodCompanyShelfItems")]
    public class SowFoodCompanyShelfItem : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string SowFoodCompanyId { get; set; }
        public string AddedById { get; set; }

        // Navigation property
        [ForeignKey("SowFoodCompanyId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual SowFoodCompany SowFoodCompany { get; set; }
        public virtual SowFoodCompanyStaff AddedBy { get; set; }
    }
}