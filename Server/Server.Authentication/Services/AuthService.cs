using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    private static readonly SigningCredentials SigningCredentials = new(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
    private static readonly SemaphoreSlim Semaphore = new(initialCount: 1, maxCount: 1);
    private static readonly JwtSecurityTokenHandler TokenHandler = new();

    private readonly ServerContext _repository;
    private readonly AuthOptions _authOptions;
    private readonly ILogger<AuthService> _logger;

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
    }

    /// <inheritdoc/>
    public async Task<OneOf<int, UserException>> RegisterAsync(
        string login,
        string password)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            _logger.LogError("Login should not be 'empty'");

            return new UserException(nameof(login).UserInvalidCredentials());
        }

        if (string.IsNullOrEmpty(password))
        {
            _logger.LogError("Password should not be 'empty'");

            return new UserException(nameof(password).UserInvalidCredentials());
        }

        var release = await Semaphore.WaitAsync(100);

        try
        {
            if (await _repository.Accounts.AnyAsync(account => account.Login.Equals(login.ToLower())))
            {
                var exceptionMessage = login.UserAlreadyExists();

                _logger.LogError("Error occured : {ExceptionMessage}", exceptionMessage);

                return new UserException(exceptionMessage);
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
                Semaphore.Release();
            }
        }
    }

    /// <inheritdoc/>
    public async Task<OneOf<AccountOutputModel, UserException>> LoginAsync(string login, string password)
    {
        var userAccount = await _repository.Accounts.FirstOrDefaultAsync(account => account.Login.ToLower().Equals(login.ToLower()));

        string exceptionMessage;

        if (userAccount is null)
        {
            exceptionMessage = login.UserNotFound();
            _logger.LogWarning("Error occured: {ExceptionMessage}", exceptionMessage);

            return new UserException(exceptionMessage);
        }

        if (login.IsCoolDown(out var coolRequestDate))
        {
            exceptionMessage = login.UserCoolDown(coolRequestDate);
            _logger.LogWarning("Error occured: {ExceptionMessage}", exceptionMessage);

            return new UserException(exceptionMessage);
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

        exceptionMessage = login.UserInvalidCredentials();
        _logger.LogWarning("Error occured: {ExceptionMessage}", exceptionMessage);

        return new UserException(exceptionMessage);
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
            SigningCredentials);

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