using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server.Authentication
{
    public class AuthOptions
    {
        public const string Issuer = "RPSGame";
        public const string Audience = "RPSPlayer";
        private const string Key = "RockPaperScissors";
        public const int Lifetime = 10;
        
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new(Encoding.ASCII.GetBytes(Key));
        }
    }
}