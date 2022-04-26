using System;
using System.Threading.Tasks;

namespace Server.Bll.Services.Interfaces;

public interface IHostedRoomService
{
    Task<int> RemoveEntityRangeByDate(TimeSpan roomOutDate, TimeSpan roundOutDate);
}