using System.Collections.Generic;
using PFR.Core.Entity;

namespace PFR.Core
{
    public class Organization
    {
        public string Name { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
