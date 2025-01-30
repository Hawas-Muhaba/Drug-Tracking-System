namespace DrugSystem.DTOs
{
    public class EditDrugDto
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public int Quantity { get; set; }
        public string PharmacyId { get; set; }
    }

}