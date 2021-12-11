using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PFR.Core.Entity;
using PFR.Core.Models;

namespace PFR.ApplicationServices.Interfaces
{
    public interface IOrganizationService
    {
        Task<List<OrganizationModel>> AddOrganization(string organizationName);

        Task<OrganizationModel> GetOrganization(Guid orgId);

        Task<List<OrganizationModel>> GetOrganizations(string searchTerm);
        
        Task AddEmployee(Guid id, AddEmployeeModel model);
    }
}