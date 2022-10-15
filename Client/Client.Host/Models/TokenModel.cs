using System.Text.Json.Serialization;

namespace Client.Host.Models;

public class TokenModel
{
    [JsonIgnore] public string BearerToken => "Bearer " + Token;
    [JsonPropertyName("Token")]
    public string Token {get; set; }
    [JsonPropertyName("Login")]
    public string Login { get; set; }
}