using System;
using System.Collections.Generic;
using System.Text;

namespace ServerlessDemo.Web.Core.Model
{
    public class Image
    {
        public int? ImageId { get; set; }
        public string RelativePath { get; set; }
        public string ThumbnailRelativePath { get; set; }
        public string OwnerUpn { get; set; }
        public DateTime AddedAt { get; set; }
        public bool Uploaded { get; set; } = false;
        public bool? Allowed { get; set; }
    }
}
