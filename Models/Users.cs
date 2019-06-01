using System;
using System.Collections.Generic;

namespace efcore1.Models
{
    public partial class Users
    {
        public Users()
        {
            Bills = new HashSet<Bills>();
            UsersToServices = new HashSet<UsersToServices>();
        }

        public int Id { get; set; }
        public string Nickname { get; set; }
        public string FullName { get; set; }
        public int GroupId { get; set; }
        public bool? IsActive { get; set; }
        public string Password { get; set; }

        public virtual UserGroups Group { get; set; }
        public virtual ICollection<Bills> Bills { get; set; }
        public virtual ICollection<UsersToServices> UsersToServices { get; set; }
    }
}
