using System;
using System.Collections.Generic;

namespace efcore1.Models
{
    public class TableProvider
    {
        public IEnumerable<Type> AvailableTables { get; set; }

        public IEnumerable<string> FieldNames { get; set; }

        public IEnumerable<IDictionary<string, object>> Values { get; set; }

        public Type EntityType { get; set; }
    }
}
