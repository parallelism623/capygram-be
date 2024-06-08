using System.Security.Claims;

namespace capygram.Auth.Services
{
    public interface IJwtServices
    {
        Task<string> GenerateAccessToken(IEnumerable<Claim> claims);
        Task<string> GenerateRefreshToken();
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
        Task SetTokenInsideCookie(string accessToken, string refreshToken, HttpContext context);
        Task RemoveTokenInsideCookie();
    }
}
