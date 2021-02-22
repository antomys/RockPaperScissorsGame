using System.Text.Json.Serialization;
namespace Server.Models.Interfaces
{
    public interface IAccount
    {
        [JsonPropertyName("Id")]
        string Id { get; }
        
        [JsonPropertyName("Login")]
        string Login { get; set; }
        
        [JsonPropertyName("Password")]
        
        string Password { get; set; }



    }
}