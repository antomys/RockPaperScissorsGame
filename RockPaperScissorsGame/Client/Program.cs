using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RockPaperScissors;
using RockPaperScissors.Server.Models;

namespace Client
{
    internal class Program
    {
        /*static async Task<int> Main(string[] args)
        {
            try
            {
                var emulator = new ClientAppEmulator();
                return await emulator.StartAsync();
            }
            catch (Exception ex)
            {
                return -1;
            }
        }*/

        public static void Main(string[] args)
        {
            var deserialize = JsonConvert.DeserializeObject<ConcurrentDictionary<string,Account>>(File.ReadAllText("Accounts.bin"));
            var guid = Guid.NewGuid();
            //var deserialize = new ConcurrentDictionary<string, Account>();
            
            var account = new Account
            {
                Id = guid.ToString(),
                Login = "xcvbbggb",
                Password = "dsfgccgh",
            };
            var stats = new Statistics
            {
                Id = account.Id,
                Wins = 46,
                Loss = 13,
                WinLossRatio = 3,
                TimeSpent = default,
                UsedRock = 0,
                UsedPaper = 0,
                UsedScissors = 0,
                Points = 0,

            };
            deserialize.TryAdd(account.Id, account);
            //deserialize.Add(account);
            var serialize = JsonConvert.SerializeObject(deserialize, Formatting.Indented);
            
            File.WriteAllText("Statistics.bin",serialize);
        }
    }
}