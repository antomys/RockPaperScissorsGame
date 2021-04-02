using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Server.Services
{
    public class Storage<T>
    {
        private readonly ConcurrentDictionary<string, T> _storage;

        public Storage()
        {
            _storage = new ConcurrentDictionary<string, T>();
        }

        public Task<bool> TryAdd(string id,T value)
        {
            return Task.FromResult(!_storage.ContainsKey(id) && _storage.TryAdd(id, value));
        }

        public Task<bool> TryRemove(string id)
        {
            return Task.FromResult(_storage.ContainsKey(id) && _storage.TryRemove(id, out _));
        }

        public Task<bool> TryUpdate(string id, T newValue)
        {
            if (!_storage.ContainsKey(id)) return Task.FromResult(false);
            _storage.TryGetValue(id, out var comparisonValue);
            return Task.FromResult(_storage.TryUpdate(id, newValue, comparisonValue));

        }
    }
}