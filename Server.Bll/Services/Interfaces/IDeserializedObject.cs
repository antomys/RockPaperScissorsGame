using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Server.Services.Interfaces
{
    public interface IDeserializedObject<T>
    {
        /// <summary>
        /// Concurrent Dictionary. Heart of our project
        /// </summary>
        ConcurrentDictionary<string, T> ConcurrentDictionary { get; set; }

        /// <summary>
        /// Method to update data in ConcurrentDictionary
        /// </summary>
        /// <returns></returns>
        Task UpdateData();
    }
}