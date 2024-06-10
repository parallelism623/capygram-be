using capygram.Common.DTOs.User;
using capygram.Common.Shared;

namespace capygram.Auth.Services
{
    public interface IUserServices
    {
        Task<Result<UserAuthenticationResponse>> Login(UserAuthenticationDto request);
        Task<Result<string>> Register(UserRegisterDto request);
        Task<Result<UserAuthenticationResponse>> ActiveAccount(UserRegisterDto request);
        Task<Result<string>> Logout(Guid Id);
        Task<Result<UserAuthenticationResponse>> RefreshToken(UserRefreshTokenDto request);
    }
}
