using capygram.Auth.Domain.Model;

namespace capygram.Auth.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task UpdateUserAsync(Guid UserId, User newUser);
        Task AddUserAsync(User user);
        Task<User> GetUserByIdAsync(Guid Id);
        Task<UserOTP> GetUserOTPByEmailAsync(string email);
        Task UpdateUserOTPAsync(Guid Id, UserOTP userOTP);
        Task AddUserOTPAsync(UserOTP userOTP);
        Task RemoveUserOTPAsync(UserOTP userOTP);
    }
}
