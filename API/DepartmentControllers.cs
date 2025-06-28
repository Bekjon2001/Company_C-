using Company.Dtos.FilterDto;
using Company.Repository.Company.Models;
using Company.Repository.Departments;
using Company.Repository.Departments.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.API.DepartmentControllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentRepository _departmentsRepository;

    public DepartmentController(IDepartmentRepository departmentsRepository)
    {
        _departmentsRepository = departmentsRepository;
    }
    // POST: api/Companies
    [HttpPost]
    public IActionResult Create([FromBody] DepartmentCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var id = _departmentsRepository.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id }, new { CompanyId = id });
    }

    // PUT: api/Companies/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] DepartmentUpdate dto)
    {
        var result = _departmentsRepository.Update(dto, id);
        if (result == null)
            return NotFound();

        return Ok(result); // frontendga to‘liq va yangilangan ma’lumot qaytadi
    }

    // DELETE: api/Companies/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _departmentsRepository.Delete(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    // GET: api/Companies/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var company = _departmentsRepository.GetById(id);
        if (company == null)
            return NotFound();

        return Ok(company);
    }

    // GET: api/Companies
    [HttpGet]
    public IActionResult GetAll([FromQuery] DepartmentFilterDto filter)
    {
        var companies = _departmentsRepository.GetAll(filter);
        return Ok(companies);
    }
}
