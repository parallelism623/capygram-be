using MongoDB.Bson.Serialization.Attributes;

namespace capygram.Auth.Domain.Model
{
    public class UserOTP
    {
        [BsonId]
        public Guid Id { get; set; }  
        public string Email { get; set; }
        public string OTP { get; set; }
        public DateTime ExpiredTimeOTP { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
