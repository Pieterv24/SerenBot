using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SerenBot.Models.Twitter
{
    public class TwitterAccessTokenResponse
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
