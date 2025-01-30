using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class DrugController : ControllerBase
{
    private readonly DrugService _drugService;

    public DrugController(DrugService drugService)
    {
        _drugService = drugService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Drug>>> Get() => await _drugService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Drug>> Get(string id)
    {
        var drug = await _drugService.GetByIdAsync(id);
        return drug is not null ? Ok(drug) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Drug drug)
    {
        await _drugService.CreateAsync(drug);
        return CreatedAtAction(nameof(Get), new { id = drug.Id }, drug);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Drug drug)
    {
        var existingDrug = await _drugService.GetByIdAsync(id);
        if (existingDrug is null) return NotFound();
        await _drugService.UpdateAsync(id, drug);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var drug = await _drugService.GetByIdAsync(id);
        if (drug is null) return NotFound();
        await _drugService.DeleteAsync(id);
        return NoContent();
    }
}
