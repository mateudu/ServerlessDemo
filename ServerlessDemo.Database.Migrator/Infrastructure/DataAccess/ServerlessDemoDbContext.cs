using System.Data.Entity;
using System.Linq;
using System.Reflection;
using ServerlessDemo.Database.Migrator.Model;

namespace ServerlessDemo.Database.Migrator.Infrastructure.DataAccess
{
    public class ServerlessDemoDbContext
        : DbContext
    {
        public ServerlessDemoDbContext()
            : this(Consts.ConnectionStrings.DocFlowMgrDbConnectionString)
        {

        }

        public ServerlessDemoDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureModel(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureModel(DbModelBuilder modelBuilder)
        {
            RegisterEntitesWithinApplication(modelBuilder);
            ApplyGlobalOverrides(modelBuilder);
        }

        private void ApplyGlobalOverrides(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties()
                .Where(p => p.Name == "Id")
                .Configure(p => p.IsKey().HasColumnName(p.ClrPropertyInfo.ReflectedType == null
                    ? "Id"
                    : p.ClrPropertyInfo.ReflectedType.Name + "Id"));

            modelBuilder.Entity<Image>().ToTable("Images");

            modelBuilder.Conventions.Add(new ForeignKeyNamingConvention());
        }

        private void RegisterEntitesWithinApplication(DbModelBuilder modelBuilder)
        {
            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");
            var entityTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => typeof(IEntity).IsAssignableFrom(x) && !x.IsAbstract);

            foreach (var type in entityTypes)
            {
                entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
            }
        }
    }
}