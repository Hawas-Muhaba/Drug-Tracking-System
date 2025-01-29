
namespace DrugSystem.DTOs
{

    public class DrugResponseDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string PharmacyName { get; set; }
        public required double Price { get; set; }
        public required int Quantity { get; set; }
        // public required double Distance { get; set; }
    }

}