using System;
using System.Threading.Tasks;
using PFR.Core.Models;

namespace PFR.ApplicationServices.Interfaces
{
    public interface IUpdateEmployeeService
    {
        Task UpdateEmployee(AddEmployeeModel model, Guid orgId);

    }
}
