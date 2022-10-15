using System.Text.Json.Serialization;
using Client.Host.Models.Interfaces;

namespace Client.Host.Models;

public class Account : IAccount
{
    [JsonPropertyName("Login")]
    public string Login { get; set; }
        
    [JsonPropertyName("Password")]
    public string Password { get; set; }
}