using System.Collections.Generic;
using Server.Bll.Models;
using Server.Dal.Entities;

namespace Server.Bll.Mapper
{
    public static class RoomMapper
    {
        public static RoomModel ToRoomModel(this Room room)
        {
            var roomPlayersModel = new RoomPlayersModel();
            var firstPlayerModel = new AccountModel();
            var secondPlayerModel = new AccountModel();
            if(room == null)
                return null;
            if (room.RoomPlayers == null)
                return new RoomModel
                {
                    Id = room.Id,
                    RoundId = room.RoundId,
                    RoomCode = room.RoomCode,
                    RoomPlayers = roomPlayersModel,
                    IsPrivate = room.IsPrivate,
                    IsReady = room.IsReady,
                    IsFull = room.IsFull,
                    CreationTime = room.CreationTime,
                    IsRoundEnded = room.IsRoundEnded,
                };
            if (room.RoomPlayers.FirstPlayer != null)
            {
                firstPlayerModel.Login = room.RoomPlayers.FirstPlayer.Login;
            }
            if (room.RoomPlayers.SecondPlayer != null)
            {
                firstPlayerModel.Login = room.RoomPlayers.SecondPlayer.Login;
            }
            roomPlayersModel = new RoomPlayersModel
            {
                FirstPlayer = firstPlayerModel,
                SecondPlayer = secondPlayerModel,
                FirstPlayerMove = room.RoomPlayers.FirstPlayerMove,
                SecondPlayerMove = room.RoomPlayers.SecondPlayerMove
            };

            return new RoomModel
            {
                Id = room.Id,
                RoundId = room.RoundId,
                RoomCode = room.RoomCode,
                RoomPlayers = roomPlayersModel,
                IsPrivate = room.IsPrivate,
                IsReady = room.IsReady,
                IsFull = room.IsFull,
                CreationTime = room.CreationTime,
                IsRoundEnded = room.IsRoundEnded,
            };
        }
    }
}