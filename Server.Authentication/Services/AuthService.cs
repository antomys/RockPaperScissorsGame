using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using Server.Authentication.Exceptions;
using Server.Authentication.Models;
using Server.Data.Context;
using Server.Data.Entities;

namespace Server.Authentication.Services;

/// <inheritdoc />
internal sealed class AuthService : IAuthService
{
    private static readonly JwtSecurityTokenHandler TokenHandler = new();
    private static SigningCredentials _signingCredentials = null!;
    
    private readonly ServerContext _repository;
    private readonly AuthOptions _authOptions;
    private readonly ILogger<AuthService> _logger;

    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/>.</param>
    /// <param name="authOptions"><see cref="AuthOptions"/>.</param>
    /// <param name="repository"><see cref="ServerContext"/>.</param>
    public AuthService(
        ILogger<AuthService> logger,
        IOptions<AuthOptions> authOptions,
        ServerContext repository)
    {
        _logger = logger;
        _repository = repository;
        _authOptions = authOptions.Value;

        _signingCredentials = new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
    }

    /// <inheritdoc/>
    public async Task<OneOf<int,UserException>>  
        RegisterAsync(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            return new UserException(nameof(login).UserInvalidCredentials());
        }
            
        if (string.IsNullOrEmpty(password))
        {
            return new UserException(nameof(password).UserInvalidCredentials());
        }
            
        var release = await _semaphore.WaitAsync(100);

        try
        {
            if (await _repository.Accounts.AnyAsync(account => account.Login.Equals(login.ToLower())))
            {
                return new UserException(login.UserAlreadyExists());
            }

            var accountId = Guid.NewGuid().ToString();
            
            var account = new Account
            {
                Id = accountId,
                Login = login,
                Password = password.EncodeBase64(),
            };
                
            _repository.Accounts.Add(account);

            var accountStatistics = new Statistics
            {
                Id = accountId,
                AccountId = accountId
            };
            
            _repository.StatisticsEnumerable.Add(accountStatistics);
            await _repository.SaveChangesAsync();

            return StatusCodes.Status200OK;
        }
        catch
        {
            _logger.LogWarning("Unable to process account for {Login}", login);
            
            return new UserException(UserExceptionsTemplates.UnknownError);
        }
        finally
        {
            if (release)
            {
                _semaphore.Release();
            }
        }
    }
        
    /// <inheritdoc/>
    public async Task<OneOf<AccountOutputModel, UserException>> LoginAsync(string login, string password)
    {
        var userAccount = await _repository.Accounts.FirstOrDefaultAsync(account => account.Login.ToLower().Equals(login.ToLower()));

        if (userAccount is null)
        {
            return new UserException(login.UserNotFound());
        }
           
        if (login.IsCoolDown(out var coolRequestDate))
        {
            return new UserException(login.UserCoolDown(coolRequestDate));
        }

        if (userAccount.Password.IsHashEqual(password))
        {
            return new AccountOutputModel
            {
                Token = BuildToken(userAccount),
                Login = userAccount.Login
            };
        }

        login.TryInsertFailAttempt();
            
        return new UserException(login.UserInvalidCredentials());
    }

    private string BuildToken(Account accountModel)
    { 
        var now = DateTime.UtcNow;

        var encodedJwt = TokenHandler.CreateEncodedJwt(
            _authOptions.Issuer,
            _authOptions.Audience,
            GetClaimsIdentity(accountModel.Id),
            now,
            now.Add(_authOptions.LifeTime),
            now,
            _signingCredentials);

        return encodedJwt;
    }

    private static ClaimsIdentity GetClaimsIdentity(string userId)
    {
        var claims = new[]
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, userId),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, Roles.User)
        };
        var claimsIdentity =
            new ClaimsIdentity(
                claims,
                JwtBearerDefaults.AuthenticationScheme,
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            
        return claimsIdentity;
    }
}