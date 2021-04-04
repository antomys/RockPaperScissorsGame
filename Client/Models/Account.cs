using System;
using Client.Models.Interfaces;
using Newtonsoft.Json;

namespace Client.Models
{
    internal class Account : IAccount
    {
        
        [JsonProperty("SessionId")]
        public string SessionId { get; set; }
        
        [JsonProperty("Login")]
        public string Login { get; set; }
        
        [JsonProperty("Password")]
        public string Password { get; set; }
        
        [JsonProperty("LastRequest")]
        public DateTime LastRequest { get; set; }
        //soon wil be deleted
        public override string ToString()
        {
            return $"Login:\t{Login}\n" +
                $"Password:\t{Password}";
        }
    }
}
