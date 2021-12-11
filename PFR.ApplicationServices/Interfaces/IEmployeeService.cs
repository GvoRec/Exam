using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PFR.Core.Models;

namespace PFR.ApplicationServices.Interfaces
{
    public interface IEmployeeService
    {
        Task AddEmployeeBatch(Guid organizationGuid, IEnumerable<AddEmployeeModel> model);
        Task<List<EmployeeModel>> GetAllEmployees(Guid organizationId);
    }
}   
