using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    public interface IDeserializedObject<T>
    {
        ConcurrentDictionary<string, T> ConcurrentDictionary { get; set; } //Just to debug. DELETE!

        Task UpdateData();


        //Task<ConcurrentDictionary<Guid, Account>> GetData();

    }
}