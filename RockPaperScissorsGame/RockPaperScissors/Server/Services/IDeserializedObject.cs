using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using RockPaperScissors.Server.Models;
using RockPaperScissors.Server.Models.Interfaces;

namespace RockPaperScissors.Server.Services
{
    public interface IDeserializedObject<T>
    {
        ConcurrentDictionary<Guid, T> ConcurrentDictionary { get; set; } //Just to debug. DELETE!

        Task UpdateData();


        //Task<ConcurrentDictionary<Guid, Account>> GetData();

    }
}