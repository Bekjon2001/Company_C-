using Company.Dtos.FilterDto;
using Company.Repository.EmployeeProjects;
using Company.Repository.EmployeeProjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.API;
[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]

public class EmployeeProjectController : Controller
{
    private readonly IEmployeeProjectRepository _employeeProjectRepository;

    public EmployeeProjectController(IEmployeeProjectRepository employeeProjectRepository)
    {
        _employeeProjectRepository=employeeProjectRepository;
    }
    // Create
    [HttpPost]
    public IActionResult Create([FromBody] EmployeeProjectCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var id = _employeeProjectRepository.Create(dto);
        return Ok(new { id });
    }
    // Update
    [HttpPut]
    public IActionResult Update([FromBody] EmployeeProjectUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = _employeeProjectRepository.Update(dto);
        if (!result) return NotFound();

        return Ok("Updated successfully");
    }
    // Delete
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _employeeProjectRepository.Delete(id);
        if (!result) return NotFound();

        return Ok("Deleted successfully");
    }
    // GetById
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var project = _employeeProjectRepository.GetById(id);
        if (project == null) return NotFound();

        return Ok(project);
    }
    // GetAll
    [HttpGet]
    public IActionResult GetAll([FromQuery] EmployeeProjectFilterDto filter)
    {
        var projects = _employeeProjectRepository.GetAll(filter);
        return Ok(projects);
    }

    [HttpGet]
    public async Task<IActionResult> Print()
    {
        var fileBytes = await _employeeProjectRepository.Print();
        return File(fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "EmployeeProject.xlsx");
    }
}
