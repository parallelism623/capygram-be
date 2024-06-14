using Amazon.Runtime.Internal.Auth;
using capygram.Auth.Domain.Model;
using capygram.Auth.Domain.Repositories;
using capygram.Auth.Domain.Services;

using capygram.Common.DTOs.User;
using capygram.Common.Exceptions;
using capygram.Common.Shared;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using capygram.Auth.MessageBus.Events;
using capygram.Common.MessageBus.Events;

namespace capygram.Auth.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtServices _jwtServices;
        private readonly IEncrypter _encrypter;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        private readonly IPublishEndpoint _publishEndpoint;
        public UserServices(IEncrypter encrypter, 
            IUserRepository userRepository, 
            IJwtServices jwtServices,
            IHttpContextAccessor httpContextAccessor,
           
            IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
            _httpContextAccessor = httpContextAccessor;
            _jwtServices = jwtServices; 
            _userRepository = userRepository; 
            _encrypter = encrypter;
            
        }

        public async Task<Result<UserAuthenticationResponse>> ActiveAccount(UserRegisterDto request)
        {
            if (request.OTP is null)
            {
                throw new BadRequestException("OTP is empty");
            }
            var userOTP = await _userRepository.GetUserOTPByEmailAsync(request.Email);
            if (userOTP is null || userOTP.ExpiredTimeOTP > DateTime.UtcNow.AddDays(1) || userOTP.OTP != request.OTP)
            {
                throw new BadRequestException("OTP is invalid");
            }
            #region Mapper
            var newUser = new User();
            newUser.UserName = request.UserName;
            newUser.Email = request.Email;
            newUser.Salt = _encrypter.GetSalt();
            newUser.Password = _encrypter.GetHash(request.Password, newUser.Salt);
            newUser.Profile.Birthday = request.Birthday;
            newUser.Profile.FullName = request.FullName;
            newUser.AccessToken = await _jwtServices.GenerateAccessToken(GetClaims(newUser));
            newUser.RefreshToken = await _jwtServices.GenerateRefreshToken();
            newUser.ExpiratimeRefreshToken = DateTime.UtcNow.AddDays(2405);
            newUser.Roles = new List<Role> { new Role { Name = "USER"} };
            #endregion
            await _userRepository.AddUserAsync(newUser);
            var result = new UserAuthenticationResponse(newUser.Id, newUser.Profile.FullName, "", newUser.Profile.FullName);
            result.AccessToken = newUser.AccessToken;
            result.RefreshToken = newUser.RefreshToken;
            
            var userNotification = new UserChangedNotification();
            userNotification.Type = "add";
            userNotification.Id = Guid.NewGuid();
            userNotification.User.Id = newUser.Id;
            userNotification.User.FullName = request.FullName;
            userNotification.User.DisplayName = request.FullName;
            using var source = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await _publishEndpoint.Publish(userNotification, source.Token);
            await _userRepository.RemoveUserOTPAsync(userOTP);
            return Result<UserAuthenticationResponse>.CreateResult(true, new ResultDetail("200", "Success"), result);
        }

        public async Task<Result<UserAuthenticationResponse>> Login(UserAuthenticationDto request)
        {
            var result = await _userRepository.GetUserByUsernameAsync(request.UserName);
            if (result == null)
            {
                throw new NotFoundException($"User not found by user name: {request.UserName}");
            }
            var checkPassword = _encrypter.GetHash(request.Password, result.Salt);
            if (checkPassword != result.Password)
            {
                throw new BadRequestException($"Password is invalid");
            }
            var claims = GetClaims(result);
            var accessToken = await _jwtServices.GenerateAccessToken(claims);
            var refreshToken = await _jwtServices.GenerateRefreshToken();
            var expirationRefreshToken = DateTime.UtcNow.AddDays(2405);
            result.AccessToken = accessToken;
            result.RefreshToken = refreshToken;
            result.ExpiratimeRefreshToken = expirationRefreshToken;
            await _userRepository.UpdateUserAsync(result.Id, result);
            var newResult = new UserAuthenticationResponse(result.Id, result.Profile.DisplayName, result.Profile.AvatarUrl, result.Profile.FullName);
            newResult.AccessToken = accessToken;
            newResult.RefreshToken = refreshToken;
            return Result<UserAuthenticationResponse>.CreateResult(true, new ResultDetail("200", "Signin successfully."), newResult);
        }
        public async Task<Result<string>> Logout(Guid Id)
        {
            var currentUser = await _userRepository.GetUserByIdAsync(Id);
            if (currentUser != null)
            {
                currentUser.RefreshToken = null;
                currentUser.AccessToken = null;
                currentUser.ExpiratimeRefreshToken = DateTime.MinValue;
                await _userRepository.UpdateUserAsync(Id, currentUser);
                return Result<string>.CreateResult(true, new ResultDetail("200", "Success"), "Logout successful");
            }
            else
            {
                return Result<string>.CreateResult(true, new ResultDetail("200", "Success"), "Logout successful");
            }
            
        }



        public async Task<Result<UserAuthenticationResponse>> RefreshToken(UserRefreshTokenDto request)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id);

            if (user.ExpiratimeRefreshToken > DateTime.UtcNow ||
                user.RefreshToken != request.RefreshToken ||
                user.AccessToken != request.AccessToken)
            {
                throw new BadRequestException("Token is invalid");
            }
            user.RefreshToken = await _jwtServices.GenerateRefreshToken();
            user.AccessToken = await _jwtServices.GenerateAccessToken(GetClaims(user));
            var resultReturn = new UserAuthenticationResponse();
            resultReturn.AccessToken = user.AccessToken;
            resultReturn.RefreshToken = user.RefreshToken;
            return Result<UserAuthenticationResponse>.CreateResult(true, new ResultDetail("200", "Success"), resultReturn);
        }

        public async Task<Result<string>> Register(UserRegisterDto request)
        {
            var newUser = new User();
            var checkUserByUserName = await _userRepository.GetUserByUsernameAsync(request.UserName);
            if (checkUserByUserName is not null)
            {
                throw new BadRequestException("Username is already exists");
            }
            var otp = new Random().Next(100000, 999999).ToString();

            // raise event + send email
            var userOTP = await _userRepository.GetUserOTPByEmailAsync(request.Email);
            if (userOTP != null)
            {
                userOTP.ExpiredTimeOTP = DateTime.Now.AddDays(1);
                userOTP.OTP = otp;
                await _userRepository.UpdateUserOTPAsync(userOTP.Id, userOTP);
            }
            else
            {
                var newUserOTP = new UserOTP();
                newUserOTP.Email = request.Email;
                newUserOTP.OTP = otp;
                newUserOTP.ExpiredTimeOTP = DateTime.Now.AddDays(1);
                await _userRepository.AddUserOTPAsync(newUserOTP);

            }
            var emailEvent = new EmailNotification()
            {
                
                Id = Guid.NewGuid(),
                Description = "Email description",
                Name = "email notification",
                TimeStamp = DateTime.Now,
                Type = "email",
                content = otp,
                mailTo = request.Email,
                nameTo = request.FullName,
            };
            using var source = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await _publishEndpoint.Publish(emailEvent, source.Token);

            return Result<string>.CreateResult(true, new ResultDetail("200", "Success"), "Send OTP success");
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
