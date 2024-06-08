using Amazon.Runtime.Internal.Auth;
using capygram.Auth.Domain.Model;
using capygram.Auth.Domain.Repositories;
using capygram.Auth.Domain.Services;
using capygram.Common.DTOs.User;
using capygram.Common.Shared;
using System.Security.Claims;

namespace capygram.Auth.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtServices _jwtServices;
        private readonly IEncrypter _encrypter;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserServices(IEncrypter encrypter, 
            IUserRepository userRepository, 
            IJwtServices jwtServices,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtServices = jwtServices; 
            _userRepository = userRepository; 
            _encrypter = encrypter;
        }
        public async Task<Result<UserAuthenticationResponse>> Login(UserAuthenticationDto request)
        {
            var result = await _userRepository.GetUserByUsernameAsync(request.UserName);
            if (result == null)
            {
                return Result<UserAuthenticationResponse>.CreateResult(false, new ResultDetail("404", "Not found user by username"), null);
            }
            if (result.IsActive == false)
            {
                return Result<UserAuthenticationResponse>.CreateResult(false, new ResultDetail("400", "User is not actived"), null);
            }
            var checkPassword = _encrypter.GetHash(request.Password, result.Salt);
            if (checkPassword != result.Password)
            {
                return Result<UserAuthenticationResponse>.CreateResult(false, new ResultDetail("400", "Password is invalid"), null);
            }
            var claims = GetClaims(result);
            var accessToken = await _jwtServices.GenerateAccessToken(claims);
            var refreshToken = await _jwtServices.GenerateRefreshToken();
            await _jwtServices.SetTokenInsideCookie(accessToken, refreshToken, _httpContextAccessor.HttpContext);
            await _userRepository.UpdateUserAsync(result.Id, result);
            var newResult = new UserAuthenticationResponse(result.Id, result.Profile.DisplayName, result.Profile.AvatarUrl, result.Profile.FullName);
            return Result<UserAuthenticationResponse>.CreateResult(true, new ResultDetail("200", "Signin successfully."), newResult);
        }

        public async Task<Result<UserAuthenticationResponse>> Register(UserRegisterDto request)
        {
            var newUser = new User();
            var checkUserByUserName = await _userRepository.GetUserByUsernameAsync(request.UserName);
            if (checkUserByUserName is not null)
            {
                return Result<UserAuthenticationResponse>.CreateResult(false, new ResultDetail("400", "Username is already exists"), null);
            }
            newUser.UserName = request.UserName;
            newUser.Email = request.Email;
            newUser.Salt = _encrypter.GetSalt();
            newUser.Password = _encrypter.GetHash(request.Password, newUser.Salt);
            newUser.Roles.Add(new Role { Name = "USER"});
            newUser.Profile.FullName = request.FullName;
            newUser.Profile.Birthday = request.Birthday;
        }
        private List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id.ToString())
            };
            foreach (var r in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Role:" + r.Name));
            }
            return claims;
        } 
    }
}
