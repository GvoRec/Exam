using System;
using System.Collections.Generic;

namespace PFR.Core.Entity
{
    public class Organization
    {
        private Organization()
        {
            
        }
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public List<Employee> Employees { get; private set; }
        
        public Organization(string organizationName)
        {
            Name = organizationName;
            Employees = new List<Employee>();
        }

        public void AddEmployee(Employee entityEmployee)
        {
            Employees.Add(entityEmployee);
        }

        public void UpdateEmployee(Employee entityEmployee)
        {
            Employees.Add(entityEmployee);
        }
    }
}