using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using Server.Authentication.Exceptions;
using Server.Authentication.Models;
using Server.Dal.Context;
using Server.Dal.Entities;

namespace Server.Authentication.Services
{
    /// <summary>
    /// Implements <see cref="IAuthService"/>.
    /// </summary>
    internal class AuthService : IAuthService
    {
        private readonly ServerContext _repository;
        private readonly AttemptValidationService _attemptValidationService;
        private readonly AuthOptions _authOptions;
        private readonly ILogger<AuthService> _logger;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        private readonly DbSet<Account> _accounts;
        

        public AuthService(
            ILogger<AuthService> logger,
            IOptions<AuthOptions> authOptions, 
            AttemptValidationService attemptValidationService, 
            ServerContext repository)
        {
            _logger = logger;
            _attemptValidationService = attemptValidationService;
            _repository = repository;
            _authOptions = authOptions.Value;
            _accounts = repository.Accounts;
        }

        /// <inheritdoc/>
        public async Task<OneOf<int,UserException>>  
            RegisterAsync(string login, string password)
        {
            if (login == null) return new UserException(UserExceptionsTemplates.UserInvalidCredentials ,nameof(login));
            if (password == null) return new UserException(UserExceptionsTemplates.UserInvalidCredentials ,nameof(password));
            
            var release = await _semaphore.WaitAsync(1000);
            try
            {
                if (await _accounts.CountAsync(x=>x.Login == login) > 0)
                    return new UserException(UserExceptionsTemplates.UserAlreadyExists,login);

                var account = new Account
                {
                    Login = login,
                    Password = password.EncodeBase64()
                };
                await _accounts.AddAsync(account);

                await _repository.SaveChangesAsync();
                
                var accountStatistics = new Statistics
                {
                    AccountId = account.Id
                };
                await _repository.StatisticsEnumerable.AddAsync(accountStatistics);
                
                await _repository.SaveChangesAsync();
                return 200;
            }
            catch
            {
                _logger.LogWarning("Unable to process account for {0}", login);
                return new UserException(UserExceptionsTemplates.UnknownError,string.Empty);
            }
            finally
            {
                if (release) 
                    _semaphore.Release();
            }
        }
        
        /// <inheritdoc/>
        //public async Task<(AccountOutputModel,DateTimeOffset)> 
        public async Task<OneOf<AccountOutputModel, UserException>> LoginAsync(string login, string password)
        {
            var userAccount = await _accounts.FirstOrDefaultAsync(x => x.Login == login);

            if (userAccount is null)
            {
                return new UserException(UserExceptionsTemplates.UserNotFound,login);
            }
           
            if (await _attemptValidationService.IsCoolDown(login, out var coolRequestDate))
                return new UserException(UserExceptionsTemplates.UserCoolDown,login,coolRequestDate);

            if (userAccount.Password.IsHashEqual(login))
                return new AccountOutputModel 
                {
                    Token = BuildToken(userAccount),
                    Login = userAccount.Login
                };
            
            await _attemptValidationService.InsertFailAttempt(login);
            return new UserException(UserExceptionsTemplates.UserInvalidCredentials, login);
        }

        public Task<bool> RemoveAsync(int accountId)
        {
            throw new NotImplementedException();
        }
        
        private string BuildToken(Account accountModel)
        {
            var identity = GetClaimsIdentity(accountModel.Id);

            var now = DateTime.UtcNow;
            var jwtToken = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(_authOptions.LifeTime),
                signingCredentials: new SigningCredentials(
                    _authOptions.GetSymmetricSecurityKey(), 
                    SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return encodedJwt;
        }

        private static ClaimsIdentity GetClaimsIdentity(int userId)
        {
            var claims = new[]
            {
                new Claim("id", userId.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
            };
            var claimsIdentity =
                new ClaimsIdentity(
                    claims,
                    "Token",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}