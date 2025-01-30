using Microsoft.AspNetCore.Mvc;
using DrugSystem.Services;
using DrugSystem.Models;
using DrugSystem.DTOs;
using MongoDB.Bson;

[ApiController]
[Route("api/[controller]")]
public class DrugsController : ControllerBase
{
    private readonly DrugService _drugService;

    public DrugsController(DrugService drugService)
    {
        _drugService = drugService;
    }

    // Validate ObjectId
    private bool IsValidObjectId(string id) => ObjectId.TryParse(id, out _);

    [HttpGet]
    public async Task<IActionResult> GetDrugs(int pageNumber = 1, int pageSize = 10)
    {
        var result = await _drugService.GetDrugsAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchDrugs(string name) => Ok(await _drugService.SearchDrugsAsync(name));

    [HttpPost]
    public async Task<IActionResult> CreateDrug([FromBody] CreateDrugDto drugDto)
    {
        try
        {
            if (drugDto == null)
            {
                return BadRequest(new { Message = "Invalid data." });
            }

            var pharmacyExists = await _drugService.CheckPharmacyExistsAsync(drugDto.PharmacyId);
            if (!pharmacyExists)
            {
                if (!ObjectId.TryParse(drugDto.PharmacyId, out _))
                {
                    return BadRequest(new { Message = "Invalid Pharmacy ID format." });
                }

                return NotFound(new { Message = "Pharmacy not found." });
            }
        
            var drug = await _drugService.CreateDrugAsync(drugDto);
            return CreatedAtAction(nameof(GetDrugs), new { id = drug.Id }, drug);
        }
         catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditDrug(string id, [FromBody] EditDrugDto drugDto)
    {
        try
        {
            var updatedDrug = await _drugService.EditDrugAsync(id, drugDto);
            return Ok(updatedDrug);
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
            return StatusCode(500, new { message = "An unexpected error occurred", error = ex.Message });
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDrug(string id)
    {
        if (!IsValidObjectId(id))
        {
            return BadRequest(new { Message = "Invalid ObjectId format." });
        }

        var deleted = await _drugService.DeleteDrugAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDrug(string id)
    {
        if (!IsValidObjectId(id))
        {
            return BadRequest(new { Message = "Invalid ObjectId format." });
        }

        var drug = await _drugService.GetDrugByIdAsync(id);
        if (drug == null)
        {
            return NotFound(new { Message = "Drug not found" });
        }
        return Ok(drug);
    }
}
