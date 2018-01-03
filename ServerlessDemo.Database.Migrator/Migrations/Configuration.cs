namespace ServerlessDemo.Database.Migrator.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ServerlessDemo.Database.Migrator.Infrastructure.DataAccess.ServerlessDemoDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ServerlessDemo.Database.Migrator.Infrastructure.DataAccess.ServerlessDemoDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
