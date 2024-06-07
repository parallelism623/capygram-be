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
        public UserServices(IEncrypter encrypter, IUserRepository userRepository, IJwtServices jwtServices)
        {
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
            var checkPassword = _encrypter.GetHash(request.Password, result.Salt);
            if (checkPassword != result.Password)
            {
                return Result<UserAuthenticationResponse>.CreateResult(false, new ResultDetail("400", "Password is invalid"), null);
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, result.Email),
                new Claim("Id", result.Id.ToString())
            };
            foreach(var r in result.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, r.Name));
            }
            var accessToken = await _jwtServices.GenerateAccessToken(claims);
            var refreshToken = await _jwtServices.GenerateRefreshToken();
            var refreshTokenSpiredAt = DateTime.UtcNow.AddDays(1);
            result.AccessToken = accessToken;
            result.RefreshToken = refreshToken;
            result.RefreshTokenExpiryTime = refreshTokenSpiredAt;
            _userRepository.UpdateUserAsync(result.Id, result);
            var newResult = new UserAuthenticationResponse();
            newResult.AccessToken = accessToken;
            newResult.RefreshToken = refreshToken;
            newResult.Id = result.Id;
            return Result<UserAuthenticationResponse>.CreateResult(true, new ResultDetail("200", "Signin successfully."), newResult);
        }
    }
}
