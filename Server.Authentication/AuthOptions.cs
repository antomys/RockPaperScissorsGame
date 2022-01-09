using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.Authentication.Models;

namespace Server.Authentication
{
    /// <summary>
    /// Options for JWT token
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// Token issuer (producer).
        /// </summary>
        public string Issuer { get; set; } = "RPC";
        
        /// <summary>
        /// Token audience (consumer).
        /// </summary>
        public string Audience { get; set; } = "Server.Host";

        /// <summary>
        /// Token secret part.
        /// </summary>
        public string PrivateKey { get; set; } = "RockPaperScissors";
        
        /// <summary>
        /// Token life time.
        /// </summary>
        public TimeSpan LifeTime { get; set; }  = TimeSpan.FromHours(3d);

        /// <summary>
        /// Require HTTPS.
        /// </summary>
        public bool RequireHttps { get; set; } = false;

        /// <summary>
        /// Getting a symmetric security key
        /// </summary>
        /// <returns></returns>
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateKey));
        }
    }
}