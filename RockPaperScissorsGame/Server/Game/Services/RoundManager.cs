/*using System;
using System.Linq;
using System.Threading.Tasks;
using Server.Game.Models;
using Server.Services.Interfaces;

namespace Server.Game.Services
{
    public class RoundManager
    {
        private readonly IDeserializedObject<Round> _deserializedRounds;

        private readonly IRoomManager _roomManager;

        public RoundManager(
            IDeserializedObject<Round> deserializedObject,
            IRoomManager roomManager)
        {
            _deserializedRounds = deserializedObject;
            _roomManager = roomManager;
        }

        public async Task<Room> StartGame(string roundId, string sessionIdThisMove, int thisMove)
        {
            var thisRoom = GetRoomByRoundId(roundId);

            if (thisRoom.Players.Any(x => x.Key.Item1 == "null"))
            {
                
            }
            
            if (thisRoom.CurrentRound.NextMove == 0)
            {
                thisRoom.CurrentRound.NextMove = thisMove;
                thisRoom.CurrentRound.SessionIdNextMove =
                    thisRoom.Players.FirstOrDefault(x => x.Key.Item1 != sessionIdThisMove).Key.Item1;

                if (UpdateRoomManager(thisRoom.RoomId, thisRoom.CurrentRound)) ;
                return thisRoom;
                
            }
            
        }
        
        private int MakeBotMove()
        {
            var v = Enum.GetValues (typeof (MoveEnum));
            var randomized = (MoveEnum) v.GetValue (new Random ().Next(v.Length));

            return (int) randomized;

        }

        private Room GetRoomByRoundId(string roundId)
        {
            return _roomManager.ActiveRooms.Values.FirstOrDefault(x => x.CurrentRound.RoundId == roundId);
        }

        private bool UpdateRoomManager(string roomId, Round round)
        {
            _roomManager.ActiveRooms.TryGetValue(roomId, out var thisRoom);
            
            var updatedRoom = thisRoom;
            updatedRoom!.CurrentRound = round;
            return _roomManager.ActiveRooms.TryUpdate(roomId, updatedRoom, thisRoom);
        }
    }
}*/