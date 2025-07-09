using Company.Data.AvtoGenerate.Entity;
using Company.Dtos.FilterDto;
using Company.Repository.Positions;
using Company.Repository.Positions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Company.API.PositionControllers;
[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]

public class PositionController : ControllerBase
{
    private readonly IPositionRepository _positionRepository;

    public PositionController(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    [HttpPost]
    public IActionResult Create([FromBody] PositionCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var id = _positionRepository.Create(dto);
        return Ok(id);
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromBody] PositionUpdate dto, int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = _positionRepository.Update(dto, id);

        if (!result)
            return NotFound($"Position with ID {id} not found.");

        return Ok("Position updated successfully.");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id) 
    {
        var result = _positionRepository.Delete(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id) 
    {
        var position = _positionRepository.GetById(id);
        if (position == null)
            return NotFound();

        return Ok(position);

    }

    [HttpGet]
    public IActionResult GetAll( [FromQuery] PositionFilterDto filter)
    {
        var result = _positionRepository.GetAll(filter);
        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> Print()
    {
        var fileBytes = await _positionRepository.Print();
        return File(fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Position.xlsx");

    }
}
