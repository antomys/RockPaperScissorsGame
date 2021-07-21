using System;
using Client.Models.Interfaces;
using Newtonsoft.Json;

namespace Client.Models
{
    public class Account : IAccount
    {
        [JsonProperty("Login")]
        public string Login { get; set; }
        
        [JsonProperty("Password")]
        public string Password { get; set; }
    }

    public class TokenModel
    {
        [JsonIgnore] public string BearerToken => "Bearer " + Token;
        [JsonProperty("Token")]
        public string Token {get; set; }
        [JsonProperty("Login")]
        public string Login { get; set; }
    }
}
