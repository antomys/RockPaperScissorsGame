using RockPaperScissors.Models.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace RockPaperScissors.Models
{
    class Account : IAccount
    {
        public Account()
        {
        }
        [JsonPropertyName("id")]
        public Guid Id { get; private set; }
        [JsonPropertyName("login")]
        public string Login { get; private set; }
        [JsonPropertyName("email")]
        public string Email { get; private set; }
        [JsonPropertyName("password")]
        public string Password { get; private set; }
    }
}
