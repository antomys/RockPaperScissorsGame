using Client.Account.Services.Interfaces;
using OneOf;
using RockPaperScissors.Common;
using RockPaperScissors.Common.Client;
using RockPaperScissors.Common.Extensions;
using RockPaperScissors.Common.Requests;
using RockPaperScissors.Common.Responses;

namespace Client.Account.Services;

internal sealed class AccountService : IAccountService
{
    private readonly IClient _client;
    private IUser _user = null!;

    public AccountService(IClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public Task<OneOf<string, CustomException>> SignUpAsync(
        string login, string password, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(login))
        {
            return Task.FromResult(OneOf<string, CustomException>.FromT1(new CustomException("Login must not be 'null' or '\"\"'")));
        }
        
        if (string.IsNullOrEmpty(password))
        {
            return Task.FromResult(OneOf<string, CustomException>.FromT1(new CustomException("Password must not be 'null' or '\"\"'")));
        }

        var request = new RegisterRequest
        {
            Login = login,
            Password = password
        };

        var response =
            _client.PostAsync<string, RegisterRequest>("api/Account/register", request, cancellationToken);

        return response;
    }
    
    public async Task<OneOf<LoginResponse, CustomException>> LoginAsync(
        string login, string password, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(login))
        {
            return new CustomException("Login must not be 'null' or '\"\"'");
        }
        
        if (string.IsNullOrEmpty(password))
        {
            return new CustomException("Password must not be 'null' or '\"\"'");
        }

        var request = new LoginRequest
        {
            Login = login,
            Password = password
        };

        var response =
            await _client.PostAsync<LoginResponse, LoginRequest>("api/Account/login", request, cancellationToken);

        if (!response.IsT0)
        {
            return response;
        }
        
        var loginResponse = response.AsT0;
        _user = new User(loginResponse.Token, loginResponse.Login);

        return response;
    }
    
    public Task<OneOf<int, CustomException>> LogoutAsync(
        string? token, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(token))
        {
            return Task.FromResult(OneOf<int, CustomException>.FromT1(new CustomException("Token must not be 'null' or '\"\"'")));
        }

        var queryUrl = new QueryBuilder(1)
        {
            { "sessionId", token }
        };
        var url = $"api/Account/logout{queryUrl}";

        var response =
            _client.GetAsync<int>(url, ("Authorization", token), cancellationToken);
        
        return response;
    }

    public IUser GetUser() => _user;
    
    public bool IsAuthorized()
    {
        return _user is not null && _user.IsAuthorized;
    }
}