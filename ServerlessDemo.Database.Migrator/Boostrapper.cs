using System.Data.Entity;
using ServerlessDemo.Database.Migrator.Infrastructure.DataAccess;

namespace ServerlessDemo.Database.Migrator
{
    public class Boostrapper
    {
        public static void ConfigureDatabase()
        {
            System.Data.Entity.Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ServerlessDemoDbContext, ServerlessDemo.Database.Migrator.Migrations.Configuration>());
        }
    }
}
