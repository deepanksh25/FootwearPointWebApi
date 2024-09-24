using FootwearPointWebApi.DataAccess;
using FootwearPointWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootwearPointWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserAuthViewModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;


                return new UserAuthViewModel
                {
                    FirstName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                };
            }
            return null;
        }

        [HttpGet("GetUserDetails")]
        [Authorize(Roles ="Admin,Customer")]
        public IResult GetUserDetails([FromBody] string email)
        {
            UserRepository repo = new UserRepository();
            UserLoginViewModel model = new UserLoginViewModel
            {
                Email = email,
                Password = ""
            };
            UserViewModel user = repo.GetUser(model);
            return Results.Ok(user);
        }
        [HttpPost("RegisterUser")]
        public IResult RegisterUser([FromBody]UserRegistrationViewModel model)
        {
            UserRepository repo = new UserRepository();
            int effRows =  repo.Insert(model);
            if (effRows > 0) {
                return Results.Ok(effRows);
            }
            else
            {
                return Results.Ok("Failed to Register User, Please try Again");
            }
        }
        [HttpPost("UpdateUser")]
        [Authorize(Roles = "Admin,Customer")]

        public IResult UpdateUser([FromBody]UserViewModel user)
        {

            UserRepository repo = new UserRepository();
            int effRows = repo.Update(user);
            if (effRows > 0)
            {
                return Results.Ok(effRows);
            }
            else
            {
                return Results.Unauthorized();
            }
        }


    }
}
