using System;
using ServerlessDemo.Database.Migrator.Infrastructure.DataAccess;

namespace ServerlessDemo.Database.Migrator.Model
{
    public class Comment : Entity<Int32>
    {
        public string OwnerUpn { get; set; }
        public virtual Image Image { get; set; }
        public DateTime AddedAt { get; set; }
        public string Text { get; set; }
        public bool? Allowed { get; set; }
    }
}
