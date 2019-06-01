using System;
using System.Collections.Generic;

namespace efcore1.Models
{
    public partial class UserGroups
    {
        public UserGroups()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
