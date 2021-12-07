

using Microsoft.VisualBasic;
using PFR.Core.Entity;

namespace PFR.Core.Models
{
    public class EmployeeModel
    {
        public long Id { get; set; }

        public string DepartmentName { get; set; }

        public string DivisionName { get; set; }

        public string Position { get; set; }

        public int Code { get; set; }

        public string FullName { get; set; }

        public static EmployeeModel MapEntity(Employee employee)
        {
            return new()
            {
                Id = employee.Id,
                FullName = Strings.Trim(string.Join(" ", employee.Surname, employee.Name, employee.Patronymic)),
                DepartmentName = employee.Department,
                DivisionName = employee.Division,
                Position = employee.Position,
                Code = employee.Code
            };

        }
    }
}