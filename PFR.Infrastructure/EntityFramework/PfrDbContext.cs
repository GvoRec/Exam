using System;
using System.Collections.Generic;
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

        public async Task<List<Organization>> GetOrganizations()
        {
            return await Set<Organization>().Include(x => x.Employees).ToListAsync();
        }

        public Task<Organization> GetOrganization(Guid orgId)
        {
            return Set<Organization>().Include(x => x.Employees)
                .SingleOrDefaultAsync(organization => organization.Id == orgId);
        }

        public async Task<List<Employee>> GetEmployees(Guid organizationId)
        {
            return (await Set<Organization>().Include(o => o.Employees).SingleAsync(o => o.Id == organizationId))
                .Employees;
        }
        
        public async Task<List<Document>> GetAllDocuments()
        {
            return await Set<Document>().ToListAsync();
        }

        public async Task<Document> GetDocument(int documentId)
        {
            return await Set<Document>().SingleAsync(doc => doc.Id == documentId);
        }
    }
}