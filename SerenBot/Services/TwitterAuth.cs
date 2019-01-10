using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SerenBot.Models.Twitter;
using System.Net.Http;
using System.Threading.Tasks;
using SerenBot.Services.Interfaces;

namespace SerenBot.Services
{
    public class TwitterAuth : IAuth
    {
        private IConfiguration _config;

        public TwitterAuth(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<string> GetAccessToken()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization",
                $"Basic {Base64Encode($"{_config.GetValue<string>("Twitter:ClientId")}:{_config.GetValue<string>("Twitter:ClientSecret")}")}");

            HttpResponseMessage response = await httpClient.PostAsync("https://api.twitter.com/oauth2/token?grant_type=client_credentials", null);

            string body = await response.Content.ReadAsStringAsync();

            var token = JsonConvert.DeserializeObject<TwitterAccessTokenResponse>(body);
            return token.AccessToken;
        }

        private object Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
