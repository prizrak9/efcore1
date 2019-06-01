using System;
using System.Collections.Generic;

namespace efcore1.Models
{
    public partial class UsersToServices
    {
        public int UserId { get; set; }
        public int ServiceId { get; set; }

        public virtual Services Service { get; set; }
        public virtual Users User { get; set; }
    }
}
