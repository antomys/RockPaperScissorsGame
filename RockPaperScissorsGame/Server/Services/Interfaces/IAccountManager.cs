using System.Collections.Concurrent;
using System.Threading.Tasks;
using Server.Contracts;
using Server.Exceptions.LogIn;
using Server.Models;

namespace Server.Services.Interfaces
{
    public interface IAccountManager
    {
        /// <summary>
        /// List of all active account on the server
        /// </summary>
        ConcurrentDictionary<string, Account> AccountsActive { get; set; }
        
        /// <summary>
        /// Method to asynchronously sign in
        /// </summary>
        /// <param name="accountDto">account from client</param>
        /// <returns>Account on server</returns>
        /// <exception cref="LoginCooldownException">When too many false retries</exception>
        /// <exception cref="InvalidCredentialsException">When invalid data</exception>
        /// <exception cref="UserAlreadySignedInException">When used is already signed in</exception>
        Task<Account> LogInAsync(AccountDto accountDto);
        
        /// <summary>
        /// Async method to sign out of account
        /// </summary>
        /// <param name="sessionId">Session id of client</param>
        /// <returns>bool</returns>
        Task<bool> LogOutAsync(string sessionId);
        
        /// <summary>
        /// Checks if this session is active
        /// </summary>
        /// <param name="sessionId">Id of client session</param>
        /// <returns>bool</returns>
        Task<bool> IsActive(string sessionId);
        
        /// <summary>
        /// Gets active account from list of active accounts by id of client session
        /// </summary>
        /// <param name="sessionId">Id of client session</param>
        /// <returns>Account</returns>
        Account GetActiveAccountBySessionId(string sessionId);
    }
}