namespace ServerlessDemo.Database.Migrator.Infrastructure.DataAccess
{
    public interface IEntity
    {

    }

    public interface IEntity<out TKey> : IEntity
    {
        TKey Id { get; }
    }
}
