using System;
using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string Rolename { get; set; }
        public bool? TagService { get; set; }
        public bool? InterfaceService { get; set; }
        public bool? IntegrationService { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
