using System;

using System.Text.Json.Serialization;

namespace Server.Models.Interfaces
{
        public interface IRoom
        {
            string Id { get; set; }
            
            public string FirstPlayerId { get; set; }
        
            public string SecondPlayerId { get; set; }
            string RoundId { get; set; }
            DateTime CreationTime { get; set; }
        }
    
}
