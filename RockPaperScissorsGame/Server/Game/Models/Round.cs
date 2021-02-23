using System;
using Server.Game.Models.Interfaces;
using Server.Models;

namespace Server.Game.Models
{
    public class Round : IRound
    {
        public string RoundId { get; init; }  //Not to store identical rounds
        public bool IsFinished { get; set; }  //Probably not needed.
        public string SessionIdNextMove { get; set; }   //Idea to store SessionId of user, that has to make move.
        
        public int NextMove { get; set; }   //enum
        
        public DateTime TimeFinished { get; set; }
        
        public Account Winner { get; set; }
        
        public Account Loser { get; set; } 
    }
}