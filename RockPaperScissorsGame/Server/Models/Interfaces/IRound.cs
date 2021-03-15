using System;
using Server.GameLogic.Models;

namespace Server.Models.Interfaces
{
    public interface IRound
    {
        string Id { get; init; }
        string RoomId { get; set; }
        public RequiredGameMove FirstPlayerMove { get; set; }
        public RequiredGameMove SecondPlayerMove { get; set; }
        bool IsFinished { get; set; }
        DateTime TimeFinished { get; set; }
        string WinnerId { get; set; }
        string LoserId { get; set; }
    }
}
