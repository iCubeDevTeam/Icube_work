using System.ComponentModel.DataAnnotations;

namespace Server.DTOs
{
    public class TokenRequestDto
    {
        [Required]
        public string GrantType { get; set; }
        // public string ClientId { get; set; }
        public string Username { get; set; }
        public int FactoryId { get; set; }
        public string RefreshToken { get; set; }
        public string Password { get; set; }
    }
}