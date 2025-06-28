using Company.Dtos.FilterDto;
using Company.Repository.Company;
using Company.Repository.Company.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Company.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICopanyRepositoriy _repository;

        public CompaniesController(ICopanyRepositoriy repository)
        {
            _repository = repository;
        }

        // POST: api/Companies
        [HttpPost]
        public IActionResult Create([FromBody] CompanyCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = _repository.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id }, new { CompanyId = id });
        }

        // PUT: api/Companies/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CompanyUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _repository.Update(dto, id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Companies/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _repository.Delete(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // GET: api/Companies/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var company = _repository.GetById(id);
            if (company == null)
                return NotFound();

            return Ok(company);
        }

        // GET: api/Companies
        [HttpGet]
        [ProducesResponseType(typeof(List<CompanyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<CompanyDto>>> GetAll([FromQuery] CompanyFilterDto filter)
        {
            var companies = await _repository.GetAllAsync(filter);
            return Ok(companies);
        }
    }
}
