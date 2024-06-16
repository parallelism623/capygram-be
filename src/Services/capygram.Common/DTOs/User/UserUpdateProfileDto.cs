namespace capygram.Common.DTOs.User
{
    public class UserUpdateProfileDto
    {
        public Guid Id { get; set; }    
        public string Story { get; set; }
        public string AvatarUrl { get; set; }
        public int Gender { get; set; }
    }
}
