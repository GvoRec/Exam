using System;
using PFR.Core.Models;

namespace PFR.Core.Entity
{
    public class Employee
    {
        private Employee()
        {
        }
        
        public Employee(AddEmployeeModel model)
        {
            Surname = model.Surname;
            Name = model.Name;
            Patronymic = model.Patronymic;
            Department = model.Department;
            Division = model.Division;
            Position = model.Position;
            Code = model.Code;
        }
        
        public Employee(UpdateEmployeeModel model)
        {
            Surname = model.Surname;
            Name = model.Name;
            Patronymic = model.Patronymic;
            Department = model.Department;
            Division = model.Division;
            Position = model.Position;
            Code = model.Code;
        }

        public int Id { get; set; }

        public string Surname { get; private set; }

        public string Name { get; private set; }

        public string Patronymic { get; private set; }

        public string Department { get; private set; }

        public string Division { get; private set; }

        public string Position { get; private set; }

        public int Code { get; private set; }

        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}