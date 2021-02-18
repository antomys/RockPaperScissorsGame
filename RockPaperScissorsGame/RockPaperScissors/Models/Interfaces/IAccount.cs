using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RockPaperScissors.Models.Interfaces
{
    internal interface IAccount
    {
        [JsonPropertyName("Id")]
        string Id { get; }
        [JsonPropertyName("Login")]
        string Login { get; }
        [JsonPropertyName("Email")]
        string Email { get; }
        [JsonPropertyName("Password")]
        string Password { get; }
    }
}
