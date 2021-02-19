using System;
using System.Collections.Concurrent;
using RockPaperScissors.Server.Models.Interfaces;

namespace RockPaperScissors.Server.Services
{
    internal interface IDeserializedObject
    {
        ConcurrentDictionary<Guid, IAccount> ConcurrentDictionary { get; set; }
    }
}