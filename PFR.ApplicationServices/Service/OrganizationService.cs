using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using PFR.ApplicationServices.Interfaces;
using PFR.Core.Entity;
using PFR.Core.Models;
using PFR.Infrastructure.EntityFramework;

namespace PFR.ApplicationServices.Service
{
    public class OrganizationService : IOrganizationService
    {
        private readonly PfrContextDbContextFactory _dbContextFactory;

        public OrganizationService(PfrContextDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<OrganizationModel>> AddOrganization(string organizationName)
        {
            var dbContext = _dbContextFactory.GetContext();
            var organization = new Organization(organizationName);
            dbContext.Add(organization);
            await dbContext.SaveChangesAsync();
            var organizations = await dbContext.GetOrganizations();
            return organizations.Select(o => new OrganizationModel
            {
                Id = o.Id,
                Employees = o.Employees.Select(employee => new EmployeeModel
                {
                    Id = employee.Id,
                    FullName = Strings.Trim(string.Join(" ", employee.Surname, employee.Name, employee.Patronymic)),
                    DepartmentName = employee.Department,
                    DivisionName = employee.Division,
                    Position = employee.Position,
                    Code = employee.Code
                }).ToList()
            }).ToList();
        }

        public async Task<OrganizationModel> GetOrganization(Guid orgId)
        {
            var dbContext = _dbContextFactory.GetContext();
            var organization = await dbContext.GetOrganization(orgId);
            return new OrganizationModel
            {
                Id = organization.Id,
                Employees = organization.Employees.Select(EmployeeModel.MapEntity).ToList()
            } ;
        }

        public async Task<List<OrganizationModel>> GetOrganizations()
        {
            var dbContext = _dbContextFactory.GetContext();
            var organizations = await dbContext.GetOrganizations();
            return organizations.Select(organization => new OrganizationModel
            {
                Id = organization.Id,
                Employees = organization.Employees.Select(EmployeeModel.MapEntity).ToList()
            }).ToList();
        }
    }
}