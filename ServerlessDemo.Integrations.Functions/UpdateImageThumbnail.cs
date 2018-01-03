using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using ServerlessDemo.Integrations.Functions.Abstract;
using ServerlessDemo.Integrations.Functions.Infrastructure;

namespace ServerlessDemo.Integrations.Functions
{
    public static class UpdateImageThumbnail
    {
        [FunctionName("UpdateImageThumbnail")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("UpdateImageThumbnail - start");
            IDataAccess db = new DataAccess();

            string body = await req.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<RequestModel>(body);

            await db.UpdateImageThumbnailRelativePathById(model.ThumbnailRelativePath, model.ImageId);

             
            return req.CreateResponse();
        }

        public class RequestModel
        {
            public int ImageId { get; set; }
            public string ThumbnailRelativePath { get; set; }
        }
    }
}
