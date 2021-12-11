using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PFR.Core.Entity;

namespace PFR.Infrastructure.EntityFramework
{
    public sealed class PfrDbContext : DbContext
    {
        public const string DefaultSchemaName = "PfrDocumentsService";

        public PfrDbContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.HasDefaultSchema(DefaultSchemaName);
        }

        public async Task<List<Organization>> GetOrganizations(string searchTerm)
        {
            var searchedOrganizations = Set<Organization>().Where(o => o.Name.Contains(searchTerm));
            return await searchedOrganizations.ToListAsync();
        }

        public Task<Organization> GetOrganization(Guid orgId)
        {
            return Set<Organization>().Include(x => x.Employees)
                .SingleOrDefaultAsync(organization => organization.Id == orgId);
        }

        public async Task<List<Employee>> GetEmployees(Guid organizationId)
        {
            var employees = Set<Employee>().Where(e => 
                e.OrganizationId == organizationId);
            return await employees.ToListAsync();
        }
        
        public async Task<List<Document>> GetAllDocuments(string searchTerm)
        {
            var searchedDocuments = Set<Document>().Where(doc => doc.Name.Contains(searchTerm));
            return await searchedDocuments.ToListAsync();
        }

        public async Task<Document> GetDocument(int documentId)
        {
            return await Set<Document>().SingleAsync(doc => doc.Id == documentId);
        }
    }
}