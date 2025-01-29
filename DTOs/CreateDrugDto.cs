

namespace DrugSystem.DTOs
{

    public class CreateDrugDto
    {
        public required string Name { get; set; }
        public required string PharmacyId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

}