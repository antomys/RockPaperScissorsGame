using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RockPaperScissors.Server.Models;
using RockPaperScissors.Server.Models.Interfaces;

namespace RockPaperScissors.Server.Services
{
    public class DeserializedObject : IDeserializedObject
    {
        private static string FileName =  "Accounts.bin";
        private static ILogger<IDeserializedObject> _logger;
        //private bool IsBusy = false;
        
        public ConcurrentDictionary<Guid, Account> ConcurrentDictionary { get; set; }

        public DeserializedObject(ILogger<IDeserializedObject> logger)
        {
            _logger = logger;
            ConcurrentDictionary = GetData().Result;
        } 

        
        

        public async Task<ConcurrentDictionary<Guid, Account>> GetData()
        {
            //var result = Deserialize().Result;
            return await Deserialize();

            /*if (results == null)
                return;*/

            /*foreach (var accountList in results)
            {
                if(accountList == null)
                    return;
                foreach (var account in accountList)  //govno
                {
                    ConcurrentDictionary.TryAdd(account., account);
                }
            }*/
        }
        
        private Task<bool> IsNeededFilesAvailable()
        { 
            return Task.Run(()=>{   
                try
                {
                    //IsBusy = true;
                    return File.Exists("Accounts.bin");
                    /*if (!File.Exists("PARSGREEN.dll"))
                        return false;*/
                }
                finally
                {
                    //IsBusy = false;
                }
            });
        }

        private async Task<ConcurrentDictionary<Guid, Account>> Deserialize()
        {
            var exists = IsNeededFilesAvailable().Result;
            
            if (exists && File.ReadAllTextAsync(FileName).Result != "")  //todo*/
                try
                {
                    using var reader = File.OpenText(FileName);
                    var fileText = await reader.ReadToEndAsync();
                    var list = await Task.Run(() => 
                        JsonConvert.DeserializeObject<ConcurrentDictionary<Guid,Account>>(fileText)); //(typeof(List<Zajecia>));
                    return list;
                }
                catch (FileNotFoundException exception)
                {
                    _logger.LogWarning($"{exception.Message}");  //todo:remove crap
                    File.Create(FileName);
                    return new ConcurrentDictionary<Guid, Account>();
                }
            else
            {
                File.Create("Accounts.bin");
                return new ConcurrentDictionary<Guid, Account>();;
            }
        }
    }
}