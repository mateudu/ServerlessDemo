using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ImageResizer;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ServerlessDemo.Integrations.Functions
{
    public static class GetThumbnail
    {
        [FunctionName("GetThumbnail")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("GetThumbnail - start request");
            var inputStream = await req.Content.ReadAsStreamAsync();

            byte[] bytes;

            // Generate a thumbnail and save it in the "thumbnails" container
            using (var outputStream = new MemoryStream())
            {
                var settings = new ResizeSettings {MaxWidth = 192, Format = "png"};
                ImageBuilder.Current.Build(inputStream, outputStream, settings);
                outputStream.Seek(0L, SeekOrigin.Begin);
                bytes = outputStream.ToArray();
            }

            var respModel = new
            {
                Bytes = bytes
            };

            return req.CreateResponse(HttpStatusCode.OK, respModel);
        }
    }
}
