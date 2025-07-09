using Company.Dtos.FilterDto;
using Company.Repository.Projects;
using Company.Repository.Projects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.API;
[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]

public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;

    public ProjectController(IProjectRepository projectRepository) // nom mos bo'lishi kerak
    {
        _projectRepository = projectRepository;
    }

    
    [HttpPost]
    public IActionResult Create([FromBody] ProjectCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var id = _projectRepository.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }


    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProjectUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = _projectRepository.Update(dto, id); // yoki await qilsang async qilib
        if (!result)
            return NotFound();

        return Ok("Successfully updated");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _projectRepository.Delete(id);
        if (!result)
            return NotFound();

        return Ok("Successfully deleted");
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var project = _projectRepository.GetById(id);
        if (project == null)
            return NotFound();
        return Ok(project);
    }
    [HttpGet]
    public IActionResult GetAll([FromQuery] ProjectFilterDto filter )
    {
        var projects = _projectRepository.GetAll(filter);
        return Ok(projects);
    }

    [HttpGet]
    public async Task<IActionResult> Print()
    {
        var fileBytes = await _projectRepository.Print();
        return File(fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Project.xlsx");
    }
}
