using System;

namespace Server.DTOs
{
    public class TokenResponseDto 
    {
        public string access_token { get; set; }
        public DateTime expiration { get; set; }
        public string refresh_token { get; set; }
        public string username { get; set; }
        
    }
}