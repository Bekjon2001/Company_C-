using Company.Dtos.FilterDto;
using Company.Repository.Leaves;
using Company.Repository.Leaves.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.API;

[Route("api/[controller]/[action]")]
[ApiController]


public class LeaveController : ControllerBase
{
    private readonly ILeaveRepositrory _leaveRepositrory;

    public LeaveController(ILeaveRepositrory leaveRepositrory)
    {
        _leaveRepositrory=leaveRepositrory;
    }
    // POST: api/LeaveControllers/Create
    [HttpPost]
    public IActionResult Create([FromBody] LeaveCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var id = _leaveRepositrory.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    // PUT: api/LeaveControllers/Update
    [HttpPut]
    public IActionResult Update([FromBody] LeaveUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ok = _leaveRepositrory.Update(dto);
        if (!ok)
            return NotFound();
        return NoContent();
    }

    // GET: api/LeaveControllers/GetById/5
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var leave = _leaveRepositrory.GetById(id);
        if (leave == null)
            return NotFound();
        return Ok(leave);
    }


    // GET: api/LeaveControllers/GetAll
    [HttpGet]
    public IActionResult GetAll( [FromQuery] LeaveFilterDto filter)
    {
        var leaves = _leaveRepositrory.GetAll(filter);
        return Ok(leaves);
    }

    // DELETE: api/LeaveControllers/Delete/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var ok = _leaveRepositrory.Delete(id);
        if (!ok)
            return NotFound();
        return NoContent();
    }
}
