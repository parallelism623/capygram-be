using capygram.Auth.DependencyInjection.Options;
using capygram.Auth.Domain.Data;
using capygram.Auth.Domain.Model;
using capygram.Auth.Domain.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace capygram.Auth.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserContext _userContext;
        public UserRepository(IUserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task AddUserAsync(User user)
        {
            await _userContext.Users.InsertOneAsync(user);
        }

        public async Task<User> GetUserByIdAsync(Guid Id)
        {
            return await _userContext.Users.Find(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var result = await _userContext.Users.Find(x => x.UserName == username).FirstOrDefaultAsync();
            return result;
        }



        public async Task UpdateUserAsync(Guid UserId, User userNew)
        {
            await _userContext.Users.ReplaceOneAsync(x => x.Id == UserId, userNew);
        }
    }
}
