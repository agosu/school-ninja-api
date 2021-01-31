using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolNinjaAPI.Data;
using SchoolNinjaAPI.DTO;
using SchoolNinjaAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using SchoolNinjaAPI.Utils;

namespace SchoolNinjaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly SchoolNinjaAPIContext _dbContext;

        public LoginController(IConfiguration config,SchoolNinjaAPIContext dbContext)
        {
            _config = config;
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpPost]
        public IActionResult Login([FromBody]LoginRequest login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJwt(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJwt(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User AuthenticateUser(LoginRequest login)
        {
            User user = (from u in _dbContext.User where u.Email == login.Email select u).FirstOrDefault();
            if (user != null)
            {
                var salt = user.Salt;
                var encodedPassword = SecretUtils.EncodePassword(login.Password, salt);
                return (from u in _dbContext.User where (u.Email == login.Email && u.Password == encodedPassword) select u).FirstOrDefault();
            }
            return null;
        }
    }
}
