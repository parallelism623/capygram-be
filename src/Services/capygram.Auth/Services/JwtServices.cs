using capygram.Auth.DependencyInjection.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace capygram.Auth.Services
{
    public class JwtServices : IJwtServices
    {
        private readonly JwtOptions _jwtOptions;
        public JwtServices(IOptionsMonitor<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.CurrentValue;
        }
        public async Task<string> GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public async Task<string> GenerateRefreshToken()
        {
            var bytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

        public async Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, 
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                ValidateLifetime = true
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }


        public async Task RemoveTokenInsideCookie(HttpContext context)
        {
            context.Response.Cookies.Append("accessToken", "", new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            // Xóa cookie "refreshToken"
            context.Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }
        public async Task SetTokenInsideCookie(string accessToken, string refreshToken, HttpContext context, bool isSetExpires=true)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };
            if (isSetExpires )
            {
                cookieOptions.Expires = DateTime.UtcNow.AddMinutes(3);
                
            }
            context.Response.Cookies.Append("accessToken", accessToken, cookieOptions);
            if (isSetExpires)
            {
                cookieOptions.Expires = DateTime.UtcNow.AddDays(2405);
            }
            context.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
