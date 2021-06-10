using System;
using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
    public partial class Factory
    {
        public Factory()
        {
            Users = new HashSet<User>();
        }

        public int FactoryId { get; set; }
        public string Factoryname { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
