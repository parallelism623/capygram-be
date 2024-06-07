using MongoDB.Bson.Serialization.Attributes;

namespace capygram.Auth.Domain.Model
{
    public class User
    {
        [BsonId]
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


    }
}
