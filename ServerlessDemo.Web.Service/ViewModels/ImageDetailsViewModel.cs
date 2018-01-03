using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerlessDemo.Web.Service.ViewModels
{
    public class ImageDetailsViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public bool IsAdmin { get; set; }
        public bool? Allowed { get; set; }
    }
}
