using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ClientId {get; set;} 
        [Required]
        public string Value { get; set; } // Value of refresh token
        [Required]
        public int UserId {get; set;}
        [Required]
        public DateTime ExpiresTime { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime? Revoked { get; set; } // desto
        public string ReplacedByToken { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresTime;
        public bool IsActive => Revoked == null && !IsExpired;

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}