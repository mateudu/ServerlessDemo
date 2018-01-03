using System.Data;
using System.Linq;
using System.Net;
using System.IO;
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
    public static class GetImageByPath
    {
        [FunctionName("GetImageByPath")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("GetImage - start");

            string body = await req.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<RequestModel>(body);

            IDataAccess db = new DataAccess();
            var img = await db.GetImageByRelativePath(model.RelativePath);

            return req.CreateResponse(img);
        }

        public class RequestModel
        {
            public string RelativePath { get; set; }
        }
    }
}
