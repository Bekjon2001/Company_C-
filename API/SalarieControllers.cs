using Company.Dtos.FilterDto;
using Company.Repository.Salaries;
using Company.Repository.Salaries.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.API;
[Route("api/[controller]/[action]")]
[ApiController]

public class SalarieController : ControllerBase
{
    private readonly ISalarieRepositoriy _salarieRepositoriy;

    public SalarieController(ISalarieRepositoriy salarieRepositoriy)
    {
        _salarieRepositoriy=salarieRepositoriy;
    }

    // POST: api/SalarieControllers/Create
    [HttpPost]
    public IActionResult Create([FromBody] SalarieCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var id = _salarieRepositoriy.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    // PUT: api/SalarieControllers/Update
    [HttpPut]
    public IActionResult Update([FromBody] SalarieUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ok = _salarieRepositoriy.Update(dto);
        if (!ok)
            return NotFound();

        return NoContent();
    }

    // GET: api/SalarieControllers/GetById/5
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var sal = _salarieRepositoriy.GetById(id);
        if (sal == null)
            return NotFound();
        return Ok(sal);
    }


    // GET: api/SalarieControllers/GetAll
    [HttpGet]
    public IActionResult GetAll([FromQuery] SalarieFilterDto filter )
    {
        var list = _salarieRepositoriy.GetAll(filter);
        return Ok(list);
    }
    // DELETE: api/SalarieControllers/Delete/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var ok = _salarieRepositoriy.Delete(id);
        if (!ok)
            return NotFound();

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> Print()
    {
        var fileBytes = await _salarieRepositoriy.Print();
        return File(fileBytes,
           "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
           "Salarie.xlsx");
    }
}
