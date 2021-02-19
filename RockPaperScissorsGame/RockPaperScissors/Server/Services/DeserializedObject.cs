using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RockPaperScissors.Server.Models;
using RockPaperScissors.Server.Models.Interfaces;

namespace RockPaperScissors.Server.Services
{
    internal class DeserializedObject : IDeserializedObject
    {
        private static string FileName =  "Accounts.bin";

        public ConcurrentDictionary<Guid, IAccount> ConcurrentDictionary { get; set; } =
            new ConcurrentDictionary<Guid, IAccount>();

        public async Task GetData()
        {
            //var result = Deserialize().Result;
            var results = await Task.WhenAll(Deserialize()); //await???

            foreach (var accountList in results)
            {
                foreach (var account in accountList)  //govno
                {
                    ConcurrentDictionary.TryAdd(account.Id, account);
                }
            }
            

        }
        
        public static async Task<List<Account>> Deserialize()
        {
            var exists = await Task.Run(() => File.Exists(FileName));
            
            if (exists && File.ReadAllTextAsync(FileName) != null)  //todo
            {
                var stream = await File.ReadAllTextAsync(FileName);
                var list = await Task.Run( () => JsonConvert.DeserializeObject<List<Account>>(stream));              //(typeof(List<Zajecia>));

                return list;
            }
            else
                return null;
        }
    }
}