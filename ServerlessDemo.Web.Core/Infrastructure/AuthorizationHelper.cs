using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using ServerlessDemo.Web.Core.Abstract;

namespace ServerlessDemo.Web.Core.Infrastructure
{
    public class AuthorizationHelper : IAadHelper
    {
        private readonly IConfiguration _configuration;

        private static readonly TokenCache TokenCache = new TokenCache();
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string Resource = "https://graph.microsoft.com/";
        
        public AuthorizationHelper(
            IConfiguration configuration
        )
        {
            _configuration = configuration;
        }

        public async Task<bool> IsAdminBySub(string sub)
        {
            var token = await GetToken();
            string adminGroup = _configuration[Consts.Groups.AdminGroup];
            var body = new
            {
                groupIds = new[]
                {
                    adminGroup
                }
            };
            var baseUri = new Uri(Resource);
            var uri = new Uri(baseUri, $"/v1.0/users/{sub}/checkMemberGroups");

            var req = new HttpRequestMessage(HttpMethod.Post, uri);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var resp = await HttpClient.SendAsync(req);
            resp.EnsureSuccessStatusCode();

            var respContent = await resp.Content.ReadAsStringAsync();
            var respBody = JsonConvert.DeserializeObject<dynamic>(respContent);
            var groups = (List<string>)respBody.value.ToObject<List<string>>();

            return groups.Contains(adminGroup);
        }

        private ClientCredential GetClientCredential()
        {
            return new ClientCredential(_configuration[Consts.AzureAd.ApplicationId], _configuration[Consts.AzureAd.Secret]);
        }

        public async Task<string> GetToken()
        {
            string authority = String.Format(_configuration[Consts.AzureAd.IssuerInstance], _configuration[Consts.AzureAd.Tenant]);
            var authContext = new AuthenticationContext(authority, TokenCache);
            var token = await authContext.AcquireTokenAsync(Resource, GetClientCredential());
            return token.AccessToken;
        }
    }
}
