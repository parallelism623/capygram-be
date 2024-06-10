using capygram.Auth.Domain.Model;
using capygram.Auth.Domain.Services;
using MongoDB.Driver;

namespace capygram.Auth.Domain.Data
{
    public static class Seeder
    {
        public static async Task Seed(IMongoCollection<User> user, IEncrypter encrypter)
        {
            bool existProduct = user.Find(p => true).Any();
            if (!existProduct)
            {
                var salt = encrypter.GetSalt();
                var password = encrypter.GetHash("admin123", salt);
                await user.InsertManyAsync(new List<User>
                {
                    new User
                    {
                        UserName = "admin",
                        Email = "admin@gmail.com",
                        Salt = salt,
                        Password = password,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true,
                        Roles = new List<Role> 
                        {
                            new Role {Name = "ADMIN"}
                        }
                    }
                });
            }
        }
    }
}

/*
 *         [BsonId]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Role> Roles { get; set; }
 */