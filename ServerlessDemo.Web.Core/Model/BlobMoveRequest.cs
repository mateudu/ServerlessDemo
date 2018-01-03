using System;
using System.Collections.Generic;
using System.Text;

namespace ServerlessDemo.Web.Core.Model
{
    public class BlobMoveRequest
    {
        public string SourceRelativePath { get; set; }
        public string DestinationRelativePath { get; set; }
    }
}
