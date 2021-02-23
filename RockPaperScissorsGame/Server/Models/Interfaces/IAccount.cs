using Newtonsoft.Json;

namespace Server.Models.Interfaces
{
    public interface IAccount
    {
        [JsonProperty("Id")]
        string Id { get; }
        
        [JsonProperty("Login")]
        string Login { get; set; }
        
        [JsonProperty("Password")]
        string Password { get; set; }



    }
}