﻿using Server.Exceptions.LogIn;
using Server.GameLogic.Exceptions;
using Server.GameLogic.Models.Impl;
using Server.Models;
using Server.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.GameLogic.LogicServices.Impl
{
    public class RoomCoordinator : IRoomCoordinator
    {
        public RoomCoordinator(IAccountManager accountManager)
        {
            _accountManager = accountManager;
            ActiveRooms = new ConcurrentDictionary<string, Room>();
            tm = CheckRoomDate;
        }
        public ConcurrentDictionary<string, Room> ActiveRooms { get; }
        public async Task<Room> CreateRoom(string sessionId, bool isPrivate)
        {
            var tasks = Task.Factory.StartNew(() =>
            {
                var account = GetAccountById(sessionId);

                if (ActiveRooms.Any(x => x.Value.Players.Any(p => p.Key.Item2.Equals(account.Login))))
                    throw new TwinkGameRoomCreationException();

                var newRoom = new Room
                {
                    RoomId = RandomString(),
                    Players = new ConcurrentDictionary<Tuple<string, string>, bool>(),  //Where Item1 = SessionId and Item2 = Login
                    CurrentRound = null,
                    IsPrivate = isPrivate,
                    IsReady = false,
                    IsRoundEnded = false,
                    IsFull = false,
                    CreationTime = DateTime.Now
                };

                if (newRoom.Players.TryAdd(new Tuple<string, string>(sessionId, account.Login), false) &&
                   newRoom.IsPrivate)
                {
                    ActiveRooms.TryAdd(newRoom.RoomId, newRoom);
                }

                _timer = new Timer(tm, null, 0, 2000);
                return newRoom;
            });
            return await tasks;
        }
        public  Task<Room> CreateTrainingRoom(string sessionId)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteRoom(string roomId)
        {
            throw new NotImplementedException();
        }
        public Task<Room> UpdateRoom(string login)
        {
            throw new NotImplementedException();
        }
        private async void CheckRoomDate(object state)
        {
            var threads = Task.Factory.StartNew(() =>
            {
                foreach (var room in ActiveRooms)
                {
                    if (room.Value.CreationTime.AddMinutes(5) < DateTime.Now && room.Value.CurrentRound == null)
                        ActiveRooms.TryRemove(room);
                }
            });
            await Task.WhenAll(threads);
        }
        private static string RandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        private Account GetAccountById(string sessionId)
        {
            _accountManager.AccountsActive.TryGetValue(sessionId, out var account);
            if (account != null) 
                return account;
            else 
                throw new UserNotFoundException(nameof(account));

        }
        private readonly IAccountManager _accountManager;
        private static readonly Random Random = new();
        private Timer _timer;
        private readonly TimerCallback tm;
    }
}
