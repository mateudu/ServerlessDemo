using System;
using ServerlessDemo.Database.Migrator.Infrastructure.DataAccess;

namespace ServerlessDemo.Database.Migrator.Model
{
    public class Image : Entity<Int32>
    {
        public string RelativePath { get; set; }
        public string ThumbnailRelativePath { get; set; }
        public string OwnerUpn { get; set; }
        public DateTime AddedAt { get; set; }
        public bool Uploaded { get; set; } = false;
        public bool? Allowed { get; set; }
    }
}
