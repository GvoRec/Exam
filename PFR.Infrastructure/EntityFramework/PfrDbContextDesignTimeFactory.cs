using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PFR.Infrastructure.EntityFramework
{
    [UsedImplicitly]
    public sealed class PfrDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PfrDbContext>
    {
        private const string DefaultConnectionString =
            "Server=127.0.0.1;Database=PfrDocumentsService;User Id = sa;Password=2wsx2WSX;";

        public static DbContextOptions<PfrDbContext> GetSqlServerOptions([CanBeNull] string connectionString)
        {
            return new DbContextOptionsBuilder<PfrDbContext>()
                .UseSqlServer(connectionString ?? DefaultConnectionString,
                    x => { x.MigrationsHistoryTable("__EFMigrationHistory", PfrDbContext.DefaultSchemaName); })
                .Options;
        }

        public PfrDbContext CreateDbContext(string[] args)
        {
            return new PfrDbContext(GetSqlServerOptions(null));
        }
    }
}