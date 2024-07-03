using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTesting.DTO_s;
using WebApiTesting.Interfaces;

namespace WebApiTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("SeedRoles")]

        public async Task<IActionResult> SeedRoles()
        {
            var funGet = await _authService.SeedRoles();
            if (funGet == false)
            {
                return Ok("roles already seeded");
            }
            return Ok("Roles seeded succefully");
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var login = await _authService.Login(dto);
            if (login == null)
            {
                return BadRequest("Wrogn Password or email");
            }
            return Ok(login);
        }


        [HttpPost("Register")]
        public async Task<IActionResult> SignUp(RegisterDTO dto)
        {
            var RegisterSucces = await _authService.SignUp(dto);
            if (RegisterSucces == false)
            {
                return BadRequest("There is a problem please try again");
            }
            return Ok("Your account has been registered succefully congrsts for entering out community");
        }


        [HttpPost("ForgotPass")]
        public async Task<IActionResult> ForgotPass(string email)
        {
            var ForgotPass = await _authService.ForgotPass(email);
            if(ForgotPass == false)
            {
                return NotFound("Email doesn't exist please register");
            }
            return Ok("An email is sent succefully you can reset now");
        }
    } 
}
