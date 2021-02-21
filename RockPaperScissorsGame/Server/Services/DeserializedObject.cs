using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Newtonsoft.Json;
using Server.Models;
using Services;

namespace Server.Services
{
    public class DeserializedObject<T> : IDeserializedObject<T> where T: class
    {
        private static string _fileName;
        private static ILogger<IDeserializedObject<T>> _logger;
        //private bool IsBusy = false;
        
        public ConcurrentDictionary<string, T> ConcurrentDictionary { get; set; }

        public DeserializedObject(ILogger<IDeserializedObject<T>> logger)
        {
            _logger = logger;
            ConcurrentDictionary = GetData().Result;
        }

        private async Task<ConcurrentDictionary<string, T>> GetData()
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
        public async Task UpdateData()
        {
            //var result = Deserialize().Result;
            await Serialize();

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
                    return File.Exists(_fileName);
                    /*if (!File.Exists("PARSGREEN.dll"))
                        return false;*/
                }
                finally
                {
                    //IsBusy = false;
                }
            });
        }

        /*private async Task<ConcurrentDictionary<string, T>> Deserialize()
        {
            if (typeof(T) == typeof(Account)) //dumb check for type
            {
                _fileName = "Account.bin";
            }

            if (typeof(T) == typeof(Statistics))
            {
                _fileName = "Statistics.bin";
            }
            
            var exists = IsNeededFilesAvailable().Result;
            
            if (exists && File.ReadAllTextAsync(_fileName).Result != "")  //todo#1#
                try
                {
                    using var reader = File.OpenText(_fileName);
                    var fileText = await reader.ReadToEndAsync();
                    var list = await Task.Run(() => 
                        JsonConvert.DeserializeObject<ConcurrentDictionary<Guid,T>>(fileText)); //(typeof(List<Zajecia>));
                    return list;
                }
                catch (FileNotFoundException exception)
                {
                    _logger.LogWarning($"{exception.Message}");  //todo:remove crap
                    File.Create(_fileName);
                    return new ConcurrentDictionary<string, T>();
                }
            else
            {
                File.Create(_fileName);
                return new ConcurrentDictionary<string, T>();;
            }
        }*/
        
        private async Task<ConcurrentDictionary<string, T>> Deserialize()
        {
            if (typeof(T) == typeof(Account)) //dumb check for type
            {
                _fileName = "Accounts.bin";
            }

            if (typeof(T) == typeof(Statistics))
            {
                _fileName = "Statistics.bin";
            }
            
            var exists = IsNeededFilesAvailable().Result;

            FileStream reader;
            if (exists && File.ReadAllTextAsync(_fileName).Result != "")  //todo*/
                try
                {
                    byte[] fileText;
                    await using (reader = File.Open(_fileName, FileMode.Open))
                    {
                        fileText = new byte[reader.Length];
                        await reader.ReadAsync(fileText, 0, (int)reader.Length);
                    }

                    var decoded = Encoding.ASCII.GetString(fileText);
                    
                    //_logger.LogInformation($"{decoded}");
                    
                    var list = await Task.Run(() => 
                        JsonConvert.DeserializeObject<ConcurrentDictionary<string,T>>(decoded)); //(typeof(List<Zajecia>));
                    return list;
                }
                catch (FileNotFoundException exception)
                {
                    _logger.LogWarning($"{exception.Message}");  //todo:remove crap
                    File.Create(_fileName);
                    return new ConcurrentDictionary<string, T>();
                }

            reader = File.Open(_fileName, FileMode.OpenOrCreate);
            reader.Close();
            return new ConcurrentDictionary<string, T>();;
            
        }

        private async Task Serialize()
        {
            var streamManager = new RecyclableMemoryStreamManager();

            using var file = File.Open(_fileName, FileMode.OpenOrCreate);
            using var memoryStream = streamManager.GetStream();
            using var writer = new StreamWriter(memoryStream);
            
            var serializer = JsonSerializer.CreateDefault();
                
            serializer.Serialize(writer, ConcurrentDictionary);      // FROM STACKOVERFLOW
                
            await writer.FlushAsync().ConfigureAwait(false);
                
            memoryStream.Seek(0, SeekOrigin.Begin);
                
            await memoryStream.CopyToAsync(file).ConfigureAwait(false);

            await file.FlushAsync().ConfigureAwait(false);
            
            /*var unicodeEncoding = new ASCIIEncoding();
            try
            {
                var list = await Task.Run(() => 
                    JsonConvert.SerializeObject(ConcurrentDictionary,Formatting.Indented));
                await File.WriteAllTextAsync("testserialization.json", list);
                var fileText = unicodeEncoding.GetBytes(list);

                await using var sourceStream = File.Open(_fileName, FileMode.OpenOrCreate);
                
                sourceStream.Seek(0, SeekOrigin.End);
                
                await sourceStream.WriteAsync(fileText, 0, fileText.Length);
            }
            catch (FileNotFoundException exception)
            {
                _logger.LogWarning($"{exception.Message}");  //todo:remove crap
                File.Create(_fileName);
            }*/
        }
    }
}