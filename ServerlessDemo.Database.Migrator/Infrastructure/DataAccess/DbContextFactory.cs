namespace ServerlessDemo.Database.Migrator.Infrastructure.DataAccess
{
    public class DbContextFactory : IDbContextFactory
    {
        public ServerlessDemoDbContext Get()
        {
            return new ServerlessDemoDbContext();
        }
    }
}