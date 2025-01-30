using DrugSystem.Models;

namespace DrugSystem.DTOs
{
    public class PharmacyResponseDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required Location Location { get; set; }
    }

}