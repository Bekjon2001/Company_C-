using Company.Repository.Atuh.Model;
using Company.Repository.Auth.Model;
using Company.Service.Atuh;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountControllers : ControllerBase
    {
        public readonly JwtService _jwtService;
        public AccountControllers(JwtService jwtService) => _jwtService = jwtService;

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseModel>>Login(LoginRequestModel request)
        {
            var result = await _jwtService.Authenticate(request);
            if (result == null) 
                return Unauthorized();
            return Ok(result);
        }

    }
}
