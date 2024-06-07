using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capygram.Common.DTOs.User
{
    public class UserAuthenticationResponse
    {
       
        public Guid Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        
    }
}
