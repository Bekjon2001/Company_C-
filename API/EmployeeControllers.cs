using Company.Dtos.FilterDto;
using Company.Repository.Employee;
using Company.Repository.Employee.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.API.EmployeeControllers;
[Route("api/[controller]/[action]")]
[ApiController]

public class EmployeeController: ControllerBase
{
    private readonly IEmployeeRepositoriy _employeeRepositoriy;

    public EmployeeController(IEmployeeRepositoriy employeeRepositoriy)
    {
        _employeeRepositoriy=employeeRepositoriy;
    }

    // POST: api/Companies
    [HttpPost]
    public IActionResult Create([FromBody] EmployeeCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var id = _employeeRepositoriy.Create(dto);

        // Natijada 201 (Created) status kodi va yaratilgan xodimni olish uchun linkni qaytaradi
        return CreatedAtAction(nameof(GetById), new { id = id }, id);
    }
    [HttpPut]
    public IActionResult Update( [FromBody] EmployeeUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = _employeeRepositoriy.Update(dto); ;
        if (!result)
            return NotFound();

        return NoContent(); // yoki Ok() ham mumkin
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _employeeRepositoriy.Delete(id);
        if (!result)
            return NotFound();

        return NoContent(); // yoki Ok()
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var employee = _employeeRepositoriy.GetById(id);
        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] EmployeeFilterDto filter)
    {
        var employees = _employeeRepositoriy.GetAll(filter);
        return Ok(employees); 
    }

}
