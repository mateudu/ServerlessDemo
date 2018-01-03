namespace ServerlessDemo.Database.Migrator.Infrastructure.DataAccess
{
    public interface IDbContextFactory
    {
        ServerlessDemoDbContext Get();
    }
}