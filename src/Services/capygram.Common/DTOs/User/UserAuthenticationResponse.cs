using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capygram.Common.DTOs.User
{
    public class UserAuthenticationResponse
    {
        public UserAuthenticationResponse() { } 
        public UserAuthenticationResponse(Guid id, string displayName, string avatarUrl, string fullName)
        {
            Id = id;
            DisplayName = displayName;
            FullName = fullName;
            AvatarUrl = avatarUrl;
        }
        public Guid Id { get; set; }
        public string? RefreshToken { get; set; }
        public string? AccessToken { get; set; }
        public string? DisplayName { get; set; } 
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        

    }
    
}
