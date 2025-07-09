using Company.Data.AvtoGenerate.Entity;
using Company.Dtos.FilterDto;
using Company.Repository.DtosExxsil;
using Company.Repository.Employee;
using Company.Repository.Employee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.API.EmployeeControllers;
[Authorize]
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

    [HttpPost]
    [Consumes("multipart/form-data")] 
    public async Task<IActionResult> ImportExcel([FromForm] UploadExcelDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("Fayl yuborilmadi.");

        var count = await _employeeRepositoriy.ImportFromExcelAsync(dto.File);
        return Ok($"{count} ta xodim import qilindi.");
    }

    [HttpGet]
    public async Task<IActionResult> ExportExcel()
    {
        var fileBytes = await _employeeRepositoriy.ExportToExcelAsync();
        return File(fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Employees.xlsx");
    }

}
