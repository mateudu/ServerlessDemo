using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ServerlessDemo.Integrations.Functions
{
    public static class GetImageMetadata
    {
        [FunctionName("GetImageMetadata")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            var bytes = await req.Content.ReadAsByteArrayAsync();
            var response = await AnalyzeImageAsync(bytes, log);

            return req.CreateResponse(response);
        }

        private async static Task<ImageAnalysisInfo> AnalyzeImageAsync(byte[] bytes, TraceWriter log)
        {
            HttpClient client = new HttpClient();

            var key = System.Environment.GetEnvironmentVariable("SubscriptionKey", EnvironmentVariableTarget.Process);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            HttpContent payload = new ByteArrayContent(bytes);
            payload.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/octet-stream");

            var endpoint = System.Environment.GetEnvironmentVariable("VisionEndpoint", EnvironmentVariableTarget.Process);
            var results = await client.PostAsync(endpoint + "/analyze?visualFeatures=Adult", payload);
            var result = await results.Content.ReadAsAsync<ImageAnalysisInfo>();
            return result;
        }

        public class ImageAnalysisInfo
        {
            public Adult adult { get; set; }
            public string requestId { get; set; }
        }

        public class Adult
        {
            public bool isAdultContent { get; set; }
            public bool isRacyContent { get; set; }
            public float adultScore { get; set; }
            public float racyScore { get; set; }
        }
    }
}
