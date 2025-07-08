using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SowFoodProject.Application.DTOs
{
    public class CreateSalesRecordDto
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ShelfItemId { get; set; }
        public string StaffId { get; set; }
        public string CompanyId { get; set; }
        public string? CustomerId { get; set; }
    }

    public class GetSalesRecordDto
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShelfItemName { get; set; }
        public string StaffName { get; set; }
        public string? CustomerName { get; set; }
        public DateTime Date { get; set; }
    }

    public class UpdateSalesRecordDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        public string StaffId { get; set; }

        [Required]
        public string ShelfItemId { get; set; }

        public string? CustomerId { get; set; }

        [Required]
        public string CompanyId { get; set; }
    }

}
