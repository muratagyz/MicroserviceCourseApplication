using Course.IdentityServer.Dtos;
using Course.IdentityServer.Models;
using Course.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Course.IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDto signupDto)
        {
            var user = new ApplicationUser()
            {
                UserName = signupDto.Username,
                Email = signupDto.Email,
                City = signupDto.City,
            };

            var result = await _userManager.CreateAsync(user, signupDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(Response<SignupDto>.Fail(errors: result.Errors.Select(x => x.Description).ToList(), statusCode: 400));
            }
            
            return NoContent();
        }
    }
}
