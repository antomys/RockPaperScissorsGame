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
        
        [JsonProperty("LastRequest")]
        public DateTime LastRequest { get; set; }
    }
}
