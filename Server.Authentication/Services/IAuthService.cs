using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OneOf;
using Server.Authentication.Exceptions;
using Server.Authentication.Models;
using Server.Data.Entities;

namespace Server.Authentication.Services;

/// <summary>
///     Authentication service.
/// </summary>
public interface IAuthService
{
    /// <summary>
    ///     Registers new entity of Client. (<see cref="Account"/>).
    /// </summary>
    /// <param name="login">Client login, case insensitive.</param>
    /// <param name="password">Client password.</param>
    /// <returns>
    /// <para>
    ///     <c>int</c> - <see cref="StatusCodes.Status200OK"/> if everything is fine.
    /// </para>
    ///     <c>UserException</c> - <see cref="UserException"/> If some case of error occured. (User exists, validation error, unknown error).
    /// </returns>
    Task<OneOf<int, UserException>> RegisterAsync(string login, string password);
    
    /// <summary>
    ///     Signs in client by credentials and building JWT token.
    /// </summary>
    /// <param name="login">Client login, case insensitive.</param>
    /// <param name="password">Client password.</param>
    /// <returns>
    /// <para>
    ///     <c>AccountOutputModel</c> - <see cref="AccountOutputModel"/> Constructed object with login and JWT token.
    /// </para>
    ///     <c>UserException</c> - <see cref="UserException"/> If some case of error occured. (User exists, validation error, unknown error).
    /// </returns>
    Task<OneOf<AccountOutputModel, UserException>> LoginAsync(string login, string password);
}