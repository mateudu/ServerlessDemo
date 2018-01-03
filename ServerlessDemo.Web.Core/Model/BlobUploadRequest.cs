using System;
using System.Collections.Generic;
using System.Text;

namespace ServerlessDemo.Web.Core.Model
{
    public class BlobUploadRequest
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
    }
}
