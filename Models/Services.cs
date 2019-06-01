using System;
using System.Collections.Generic;

namespace efcore1.Models
{
    public partial class Services
    {
        public Services()
        {
            UsersToServices = new HashSet<UsersToServices>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }

        public virtual ICollection<UsersToServices> UsersToServices { get; set; }
    }
}
