using DrugSystem.Models;
using System.ComponentModel.DataAnnotations; 

namespace DrugSystem.DTOs
{
    public class EditPharmacyDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Location Location { get; set; } = new Location();
    }

}