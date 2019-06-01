using System;
using System.Collections.Generic;

namespace efcore1.Models
{
    public partial class Bills
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Cost { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }

        public virtual Users User { get; set; }
    }
}
