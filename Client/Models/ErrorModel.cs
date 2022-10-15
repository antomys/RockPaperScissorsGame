using Newtonsoft.Json;

namespace Client.Models;

public class ErrorModel
{
    [JsonProperty("Code")]
    public int Code { get; set; }
    [JsonProperty("Message")]
    public string Message { get; set; }
}