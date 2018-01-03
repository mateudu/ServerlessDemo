using System;
using System.Collections.Generic;
using System.Text;

namespace ServerlessDemo.Web.Core.Model
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string OwnerUpn { get; set; }
        public virtual Image Image { get; set; }
        public DateTime AddedAt { get; set; }
        public string Text { get; set; }
        public bool? Allowed { get; set; }
        public int ImageId { get; set; }
    }
}
