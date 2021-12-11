using System;
using System.Collections.Generic;

namespace PFR.Core.Models
{
    public class OrganizationModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<EmployeeModel> Employees {get; set; }
    }
}
