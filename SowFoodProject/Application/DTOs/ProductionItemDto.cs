using System.ComponentModel.DataAnnotations;

namespace SowFoodProject.Application.DTOs
{
    public class CreateProductionItemDto
    {
        [Required]
        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string ImageUrl { get; set; }
        public string ProductionStaffId { get; set; }

        public DateTime DateOfProduction { get; set; }
        public required string AddedBy { get; set; }

    }

    public class UpdateProductionItemDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DateOfProduction { get; set; }

        [Required]
        public string CompanyId { get; set; }
    }

    public class GetProductionItemDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DateOfProduction { get; set; }

        public string CompanyId { get; set; }
    }

}
