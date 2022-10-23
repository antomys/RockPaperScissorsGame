using System.Text.Json.Serialization;

namespace RockPaperScissors.Common;

public sealed class CustomException
{
    [JsonConstructor]
    public CustomException(int code, string message)
    {
        Message = message;
        Code = code;
    }
    
    public CustomException(string message)
    {
        Message = message;
        Code = 400;
    }
    
    [JsonPropertyName("code")]
    public int Code { get; init; }
   
    [JsonPropertyName("message")]
    public string Message { get; init; }
}