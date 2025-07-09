using Company.Data.dataContext;
using Company.Migrations;
using Company.Repository.Atuh.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Company.Dost;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Drawing.Imaging;
using System.Text;
using System.Security.Cryptography;
using Company.Repository.Auth.Model;

namespace Company.Service.Atuh
{
    public class JwtService 
    {
        private readonly DbContextdta _dbContext;
        private readonly IConfiguration _configuration;
        public  JwtService(DbContextdta dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<LoginResponseModel?> Authenticate(LoginRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return null;
            var userAccount = await _dbContext.UserAccounts.FirstOrDefaultAsync(x => x.Username == request.Username);
            if (userAccount is null || !PasswordHashHandlar.VerifyPaswword(request.Password, userAccount.Password))
                return null;

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];

            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, request.Username),
                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials =  new SigningCredentials(
                                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                                        SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandlar = new JwtSecurityTokenHandler();
            var securityTokent = tokenHandlar.CreateToken(tokenDescriptor);
            var accessToken = tokenHandlar.WriteToken(securityTokent);

            return new LoginResponseModel
            {
                AccessToken = accessToken,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }



    }
}
