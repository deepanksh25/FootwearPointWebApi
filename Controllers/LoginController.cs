using FootwearPointWebApi.DataAccess;
using FootwearPointWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FootwearPointWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        public LoginController(IConfiguration config) { 
        _configuration = config;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLoginViewModel model) { 
        UserViewModel user = Authenticate(model);
            UserAuthViewModel auth = new UserAuthViewModel();
            if (user != null) {
                var token = Generate(user);
                auth.FirstName = user.FirstName;
                auth.Surname = user.LastName;
                auth.Email = user.Email;
                auth.token = token;
                auth.Role = user.RoleID == 1 ? "Admin" : "Customer";
                return Ok(auth);
            }
            return NotFound("User not Found");
        }
        private string Generate(UserViewModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            string role = "";
            if(user.RoleID == 1)
            {
                role = "Admin";
            }
            else
            {
                role = "Customer";
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.FirstName),
                 new Claim(ClaimTypes.Email,user.Email),
                  new Claim(ClaimTypes.Surname,user.LastName),
                   new Claim(ClaimTypes.Role,role),
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],

                _configuration["Jwt:Audience"],
                claims,
                expires:DateTime.Now.AddMinutes(20),
                signingCredentials:credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private UserViewModel Authenticate(UserLoginViewModel model) { 
            UserRepository userRepository = new UserRepository();
            UserViewModel user = userRepository.GetUser(model);
            if (user == null) {
                return null;
            }
            return user;
        }
    }
}
