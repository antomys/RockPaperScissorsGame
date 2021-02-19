using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using RockPaperScissors.Server.Models;
using RockPaperScissors.Server.Models.Interfaces;

namespace RockPaperScissors.Server.Services
{
    public interface IDeserializedObject
    {
        ConcurrentDictionary<Guid, Account> ConcurrentDictionary { get; set; }


        //Task<ConcurrentDictionary<Guid, Account>> GetData();
        
    }
}