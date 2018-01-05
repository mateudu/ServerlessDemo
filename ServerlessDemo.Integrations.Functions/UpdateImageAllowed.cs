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
    public static class UpdateImageAllowed
    {
        [FunctionName("UpdateImageAllowed")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("UpdateImageAllowed: START");

            var data = await req.Content.ReadAsStringAsync();

            log.Info("UpdateImageAllowed: Parse request message model");
            var requestMsg = JsonConvert.DeserializeObject<RequestModel>(data);
            
            IDataAccess dataAccess = new DataAccess();

            log.Info($"UpdateImageAllowed: Update image allowed for imageId={requestMsg.ImageId}");
            await dataAccess.UpdateImageAllowedById(requestMsg.Allowed, requestMsg.ImageId);

            log.Info("UpdateImageAllowed: END");
            return req.CreateResponse();
        }

        public class RequestModel
        {
            public int ImageId { get; set; }
            public bool Allowed { get; set; }
        }
    }
}
