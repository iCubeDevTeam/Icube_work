using System;
using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Passwordhash { get; set; }
        public byte[] Passwordsalt { get; set; }
        public int? RoleId { get; set; }
        public int? FactoryId { get; set; }

        public virtual Factory Factory { get; set; }
        public virtual Role Role { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
