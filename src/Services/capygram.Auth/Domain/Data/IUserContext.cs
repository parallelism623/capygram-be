using capygram.Auth.Domain.Model;
using MongoDB.Driver;

namespace capygram.Auth.Domain.Data
{
    public interface IUserContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<UserOTP> UserOTPs { get; }
    }
}
