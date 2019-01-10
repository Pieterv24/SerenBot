using Newtonsoft.Json;
using SerenBot.Models;
using SerenBot.Models.Twitter;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SerenBot.Services.Interfaces;

namespace SerenBot.Services
{
    public class VosService : IVosService
    {
        private readonly string ScreenName = "JagexClock";
        private readonly int Count = 10;

        private readonly IAuth _authService;

        public VosService(IAuth authService)
        {
            _authService = authService;
        }

        public async Task<List<Status>> GetVosStatuses()
        {
            string token = await _authService.GetAccessToken();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            string body = await httpClient.GetStringAsync(
                $"https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={ScreenName}&count={Count}");

            List<Status> statuses = JsonConvert.DeserializeObject<List<Status>>(body);
            return statuses;
        }

        public async Task<VoiceOfSeren[]> GetActiveVos()
        {
            List<Status> statuses = await GetVosStatuses();
            VoiceOfSeren[] voices = new VoiceOfSeren[2];
            Regex vosRegex = new Regex(@"The Voice of Seren is now active in the (\w+) and (\w+) districts.*");
            foreach (Status status in statuses)
            {
                Match match = vosRegex.Match(status.Text);
                if (match.Success)
                {
                    Enum.TryParse(match.Groups[1].Value, true, out voices[0]);
                    Enum.TryParse(match.Groups[2].Value, true, out voices[1]);
                    break;
                }
            }

            return voices;
        }
    }
}
