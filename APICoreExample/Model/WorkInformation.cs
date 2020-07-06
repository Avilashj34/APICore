using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICoreExample.Model
{
    public class WorkInformation:BaseEntity
    {
        public string CompanyName { get; set; }
        public int Experience { get; set; }
        public string Position { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
