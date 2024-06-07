using capygram.Auth.Domain.Repositories;
using capygram.Auth.Services;
using capygram.Common.DTOs.User;
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
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserAuthenticationDto request)
        {
            var result = await _userServices.Login(request);
            return Ok(result);
        }
    }
}
