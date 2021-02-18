using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using RockPaperScissors.Models;

namespace RockPaperScissors.Services.ServerServices
{
    public interface IAccountManager<T> where T: class
    {
        Task<IEnumerable<ItemWithId<T>>> GetAllAsync();

        Task<T> GetAsync(string nickName, string password);

        Task<int> AddAsync(T item);

        Task<bool> DeleteAsync(string id);
        
    }
}