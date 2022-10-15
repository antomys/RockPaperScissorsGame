using System.Text.Json.Serialization;

namespace Client.Host.Models;

public class ErrorModel
{
    [JsonPropertyName("Code")]
    public int Code { get; set; }
    [JsonPropertyName("Message")]
    public string Message { get; set; }
}