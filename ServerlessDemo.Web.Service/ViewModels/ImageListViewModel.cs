using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerlessDemo.Web.Service.ViewModels
{
    public class ImageListViewModel
    {
        public List<Image> Images { get; set; } = new List<Image>();
        public class Image
        {
            public int Id { get; set; }
            public string Url { get; set; }
            public string ThumbnailUrl { get; set; }
        }
    }

    
}
