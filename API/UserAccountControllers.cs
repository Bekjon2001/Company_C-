using Company.Data.AvtoGenerate.Entity;
using Company.Data.dataContext;
using Company.Dost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserAccountControllers : ControllerBase
    {
        public readonly DbContextdta _dbContext;

        public UserAccountControllers(DbContextdta dbContext)
        {
            _dbContext=dbContext;
        }
        [HttpGet]
        public async Task<List<UserAccount>> Get()
        {
            return await _dbContext.UserAccounts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<UserAccount?>GetByid(int id)
        {
            return await _dbContext.UserAccounts.FirstOrDefaultAsync(x => x.UserId == id);  
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserAccount user)
        {
            user.Password = PasswordHashHandlar.HashPaswword(user.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _dbContext.UserAccounts.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok(user);
        }
    }
}
