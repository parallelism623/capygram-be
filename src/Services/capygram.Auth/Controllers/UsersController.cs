using capygram.Auth.Domain.Repositories;
using capygram.Auth.Services;
using capygram.Common.DTOs.User;
using capygram.Common.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace capygram.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        
        private readonly IUserServices _userServices;
        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAuthenticationDto request)
        {
            var result = await _userServices.Login(request);
            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            var result = await _userServices.Register(request);
            return Ok(result); 
        }
        [HttpPost("logout")]
        [MustHaveRole("Role:USER")]
        public async Task<IActionResult> Logout(Guid Id)
        {
            var result = await _userServices.Logout(Id);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [MustHaveRole("Role:USER")]
        public async Task<IActionResult> RefreshToken(UserRefreshTokenDto request)
        {
            var result = await _userServices.RefreshToken(request);
            return Ok(result);
        }
        [HttpPost("active-account")]
        public async Task<IActionResult> ActiveAccount(UserRegisterDto request)
        {
            var result = await _userServices.ActiveAccount(request);
            return Ok(result);
        }
    }
}
