using DrugSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace DrugSystem.DTOs
{

    public class CreatePharmacyDto
    {
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name field cannot exceed 100 characters.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "The Location field is required.")]
        public required Location Location { get; set; }
    }
}
