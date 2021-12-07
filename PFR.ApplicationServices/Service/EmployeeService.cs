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
    public class EmployeeService : IEmployeeService
    {
        private readonly PfrContextDbContextFactory _dbContextFactory;

        public EmployeeService(PfrContextDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddEmployee(AddEmployeeModel model)
        {
            var entityEmployee = new Employee(model);
            var dbContext = _dbContextFactory.GetContext();
            var organization = await dbContext.GetOrganization(model.OrganizationId);
            organization.AddEmployee(entityEmployee);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddEmployeeBatch(IEnumerable<AddEmployeeModel> model)
        {
            var dbContext = _dbContextFactory.GetContext();
            var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                foreach (var addEmployeeModel in model)
                {
                    var entityEmployee = new Employee(addEmployeeModel);
                    var organization = await dbContext.GetOrganization(addEmployeeModel.OrganizationId);
                    organization.AddEmployee(entityEmployee);
                }

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<EmployeeModel>> GetAllEmployees(Guid organizationId)
        {
            return (await _dbContextFactory.GetContext().GetEmployees(organizationId)).Select(EmployeeModel.MapEntity).ToList();
        }
    }
}