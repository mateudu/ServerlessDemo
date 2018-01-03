using System;
using System.ComponentModel.DataAnnotations;

namespace ServerlessDemo.Database.Migrator.Infrastructure.DataAccess
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        [Key]
        public TKey Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<TKey>);
        }

        public virtual bool Equals(Entity<TKey> other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                       otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(Int64)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<TKey> x, Entity<TKey> y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Entity<TKey> x, Entity<TKey> y)
        {
            return !(x == y);
        }

        private static bool IsTransient(Entity<TKey> obj)
        {
            return obj != null && Equals(obj.Id, default(TKey));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }
    }
}
