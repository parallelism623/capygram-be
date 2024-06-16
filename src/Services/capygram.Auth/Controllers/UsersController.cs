using capygram.Auth.Domain.Repositories;
using capygram.Auth.Services;
using capygram.Common.DTOs.User;
using capygram.Common.Filters;
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
        [ValidationModel]
        public async Task<IActionResult> Login([FromBody] UserAuthenticationDto request)
        {
            var result = await _userServices.Login(request);
            return Ok(result);
        }
        [HttpPost("register")]
        [ValidationModel]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            var result = await _userServices.Register(request);
            return Ok(result); 
        }
        [HttpPost("logout")]
        [MustHaveRole("Role:USER")]
        [ValidationModel]
        public async Task<IActionResult> Logout(Guid Id)
        {
            var result = await _userServices.Logout(Id);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [MustHaveRole("Role:USER")]
        [ValidationModel]
        public async Task<IActionResult> RefreshToken(UserRefreshTokenDto request)
        {
            var result = await _userServices.RefreshToken(request);
            return Ok(result);
        }
        [HttpPost("active-account")]
        [ValidationModel]
        public async Task<IActionResult> ActiveAccount(UserRegisterDto request)
        {
            var result = await _userServices.ActiveAccount(request);
            return Ok(result);
        }
        [HttpPost("edit")]
        [ValidationModel]
        public async Task<IActionResult> UpdateProfile(UserUpdateProfileDto request)
        {
            var result = await _userServices.UpdateProfile(request);
            return Ok(result);
        }
    }
}
