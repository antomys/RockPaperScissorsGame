using RockPaperScissors.Models.Interfaces;
using System;
using System.Text.Json.Serialization;
using RockPaperScissors.Server.Models;

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

        //[JsonPropertyName("id")]
        //public Guid Id { get; private set; }
        //[JsonPropertyName("firstName")]
        //public string FirstName { get; private set; }
        //[JsonPropertyName("lastName")]
        //public string LastName { get; private set; }
        [JsonPropertyName("login")]
        public string Login { get; set; }
        //[JsonPropertyName("email")]
        //public string Email { get; private set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        
        
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
