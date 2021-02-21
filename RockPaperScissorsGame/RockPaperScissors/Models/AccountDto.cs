using System;
using RockPaperScissors.Models.Interfaces;
using Newtonsoft.Json;

namespace RockPaperScissors.Models
{
    public class AccountDto : IAccountDto
    {
        /*public Account(string login,string email, string password, string firstName, string lastName )
        {
            Id = Guid.NewGuid();
            Login = login;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }*/

        [JsonProperty("SessionId")]
        public string SessionId { get; set; }
        //[JsonPropertyName("firstName")]
        //public string FirstName { get; private set; }
        //[JsonPropertyName("lastName")]
        //public string LastName { get; private set; }
        [JsonProperty("Login")]
        public string Login { get; set; }
        //[JsonPropertyName("email")]
        //public string Email { get; private set; }
        
        [JsonProperty("Password")]
        public string Password { get; set; }
        
        [JsonProperty("LastRequest")]
        
        public DateTime LastRequest { get; set; }
        
        
        //Soon will be deleted(for testing mode!)
        /*public override string ToString()
        {
            return $"Guid: {Id}\n" +
                $"Firstname: {FirstName}\n" +
                $"Lastname: {LastName}\n" +
                $"Login: {Login}\n" +
                $"Email: {Email}\n" +
                $"Password: {Password}";
        }*/
    }
}
