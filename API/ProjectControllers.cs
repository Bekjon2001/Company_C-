using Company.Dtos.FilterDto;
using Company.Repository.Projects;
using Company.Repository.Projects.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.API;
[Route("api/[controller]/[action]")]
[ApiController]

public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;

    public ProjectController(IProjectRepository projectRepository) // nom mos bo'lishi kerak
    {
        _projectRepository = projectRepository;
    }

    // POST: api/ProjectControllers/Create
    [HttpPost]
    public IActionResult Create([FromBody] ProjectCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var id = _projectRepository.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    // PUT: api/ProjectControllers/Update
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

    // DELETE: api/ProjectControllers/Delete/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _projectRepository.Delete(id);
        if (!result)
            return NotFound();

        return Ok("Successfully deleted");
    }

    // GET: api/ProjectControllers/GetById/5
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
}
