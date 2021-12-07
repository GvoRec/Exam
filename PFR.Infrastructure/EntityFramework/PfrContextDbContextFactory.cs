namespace PFR.Infrastructure.EntityFramework
{
   public class PfrContextDbContextFactory
    {

        private readonly string _connectionString;

        public PfrContextDbContextFactory (string connectionString)
        {
            _connectionString = connectionString;

        }

        public PfrDbContext GetContext()
        {
            return new PfrDbContext(PfrDbContextDesignTimeFactory.GetSqlServerOptions(_connectionString));
        }

    }
}
