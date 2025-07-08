﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SowFoodProject.Models.Entities;

namespace SowFoodProject.Models.Entities.SowFoodLinkUp
{
    [Table("SowFoodCompanyProductionItems")]
    public class SowFoodCompanyProductionItem : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public string ImageUrl { get; set; }
        public string AddedById { get; set; }
        public DateTime DateOfProduction { get; set; }

        [Required]
        public string SowFoodCompanyId { get; set; }

        [ForeignKey("SowFoodCompanyId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual SowFoodCompany SowFoodCompany { get; set; }
        public virtual SowFoodCompanyStaff AddedBy { get; set; }
    }
}