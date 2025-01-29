using Microsoft.AspNetCore.Mvc;
using DrugSystem.DTOs;
using DrugSystem.Services;
using DrugSystem.Models;
using MongoDB.Bson;

namespace DrugSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmaciesController : ControllerBase
    {
        private readonly PharmacyService _pharmacyService;

        public PharmaciesController(PharmacyService pharmacyService)
        {
            _pharmacyService = pharmacyService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePharmacy([FromBody] CreatePharmacyDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                      .Select(e => e.ErrorMessage)
                                                      .ToList();
                return BadRequest(new { Errors = errorMessages });
            }

            var pharmacy = await _pharmacyService.CreatePharmacyAsync(dto);
            return CreatedAtAction(nameof(GetPharmacy), new { id = pharmacy.Id }, pharmacy);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditPharmacy(string id, [FromBody] EditPharmacyDto pharmacyDto)
        {
            try
            {
                var updatedPharmacy = await _pharmacyService.EditPharmacyAsync(id, pharmacyDto);
                return Ok(updatedPharmacy);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the pharmacy.", details = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPharmacy(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest(new { Message = "Invalid MongoDB ObjectId format" });
            }

            var pharmacy = await _pharmacyService.GetPharmacyByIdAsync(id);
            if (pharmacy == null)
            {
                return NotFound(new { Message = "Pharmacy not found" });
            }
            return Ok(pharmacy);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPharmacies(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _pharmacyService.GetAllPharmaciesAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePharmacy(string id)
        {
            var success = await _pharmacyService.DeletePharmacyAsync(id);
            if (!success)
            {
                return NotFound(new { Message = "Pharmacy not found" });
            }
            return NoContent();
        }
    }
}
