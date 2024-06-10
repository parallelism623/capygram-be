namespace capygram.Common.DTOs.User
{
    public class UserRefreshTokenDto
    {
        public Guid Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
