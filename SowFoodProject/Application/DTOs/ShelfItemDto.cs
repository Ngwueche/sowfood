namespace SowFoodProject.Application.DTOs
{
    public class CreateShelfItemDto
    {
        public string Name { get; set; }
        public string ShelfStaffId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ImageUrl { get; set; }
    }

    public class UpdateShelfItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ImageUrl { get; set; }
    }

    public class GetShelfItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ImageUrl { get; set; }
        public string CompanyId { get; set; }
        public DateTime AddedAt { get; set; }
    }

}
