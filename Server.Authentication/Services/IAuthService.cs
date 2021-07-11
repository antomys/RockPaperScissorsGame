using System;
using System.Threading.Tasks;
using OneOf;
using Server.Authentication.Exceptions;
using Server.Authentication.Models;

namespace Server.Authentication.Services
{
    public interface IAuthService
    {
        Task<OneOf<int, UserException>> RegisterAsync(string login, string password);
        Task<OneOf<AccountOutputModel, UserException>> LoginAsync(string login, string password);
    }
}