using capygram.Common.DTOs.User;
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
        public DateTime ExpiratimeRefreshToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = false;
        public ICollection<Role> Roles { get; set; } = new List<Role>();
        public Profile Profile { get; set; } = new Profile();


    }
}
