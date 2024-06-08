using capygram.Common.DTOs.User;
using capygram.Common.Shared;

namespace capygram.Auth.Services
{
    public interface IUserServices
    {
        Task<Result<UserAuthenticationResponse>> Login(UserAuthenticationDto request);
        Task<Result<UserAuthenticationResponse>> Register(UserRegisterDto request);
    }
}
