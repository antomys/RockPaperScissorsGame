using Newtonsoft.Json;

namespace Client.Models;

public class TokenModel
{
    [JsonIgnore] public string BearerToken => "Bearer " + Token;
    [JsonProperty("Token")]
    public string Token {get; set; }
    [JsonProperty("Login")]
    public string Login { get; set; }
}