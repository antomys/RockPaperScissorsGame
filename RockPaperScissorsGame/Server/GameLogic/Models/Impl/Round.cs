﻿using System;
using System.Collections.Concurrent;

namespace Server.GameLogic.Models.Impl
{
    public class Round : IRound
    {
        public string Id { get; init; }

        public string RoomId { get; set; }
        public bool IsFinished { get; set; }
        
        public ConcurrentDictionary<string, RequiredGameMove>  PlayerMoves { get; set; }
        
        public bool IsDraw { get; set; }
        
        public DateTime TimeFinished { get; set; }
        
        public string WinnerId { get; set; }
        
        public string LoserId { get; set; }
        
    }
}
