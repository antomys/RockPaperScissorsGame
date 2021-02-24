using System;
using System.Collections.Concurrent;

namespace Server.GameLogic.Models.Impl
{
    public class Round : IRound
    {
        public string RoundId { get; init; }
        
        public bool IsFinished { get; set; }
        
        public ConcurrentDictionary<string, int>  Moves { get; set; }
        
        public DateTime TimeFinished { get; set; }
        
        public string WinnerId { get; set; }
        
        public string LoserId { get; set; }
    }
}
