using capygram.Auth.DependencyInjection.Options;
using capygram.Auth.Domain.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace capygram.Auth.Domain.Data
{
    public class UserContext : IUserContext
    {
        public UserContext(IOptionsMonitor<UserDBSetting> optionsMonitor) 
            {
            var userDBSetting = optionsMonitor.CurrentValue;
            var mongoClient = new MongoClient(userDBSetting.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(userDBSetting.DatabaseName);
            Users = mongoDatabase.GetCollection<User>(userDBSetting.UserCollectionName);
            var indexKeysDefinition = Builders<User>.IndexKeys.Ascending(x => x.UserName);
            Users.Indexes.CreateOne(new CreateIndexModel<User>(indexKeysDefinition));
            UserOTPs = mongoDatabase.GetCollection<UserOTP>(userDBSetting.UserOTPCollectionName);
        }

        public IMongoCollection<User> Users { get; }

        public IMongoCollection<UserOTP> UserOTPs { get; }
    }
}
