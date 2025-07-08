﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SowFoodProject.Models.Entities;

namespace SowFoodProject.Models.Entities.SowFoodLinkUp
{
    [Table("SowFoodCompanySalesRecords")]
    public class SowFoodCompanySalesRecord : BaseEntity
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Quantity * UnitPrice;

        public string? SowFoodCompanyProductItemId { get; set; }
        public string? SowFoodCompanyShelfItemId { get; set; }
        public string? SowFoodCompanyCustomerId { get; set; }
        public string SowFoodCompanyId { get; set; }

        [Required]
        public string SowFoodCompanyStaffId { get; set; }

        [ForeignKey("SowFoodCompanyProductItemId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual SowFoodCompanyProductionItem? SowFoodCompanyProductItem { get; set; }

        [ForeignKey("SowFoodCompanyShelfItemId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual SowFoodCompanyShelfItem? SowFoodCompanyShelfItem { get; set; }

        [ForeignKey("SowFoodCompanyCustomerId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual SowFoodCompanyCustomer SowFoodCompanyCustomer { get; set; }

        [ForeignKey("SowFoodCompanyStaffId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual SowFoodCompanyStaff SowFoodCompanyStaff { get; set; }

        [ForeignKey("SowFoodCompanyId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual SowFoodCompany SowFoodCompany { get; set; }
    }
}