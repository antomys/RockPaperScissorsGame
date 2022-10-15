using OneOf;
using RockPaperScissors.Common;
using RockPaperScissors.Common.Responses;

namespace Client.Account.Services;

public interface IAccountService
{
    Task<OneOf<string, CustomException>> SignUpAsync(string login,
        string password,
        CancellationToken cancellationToken = default);

    Task<OneOf<LoginResponse, CustomException>> LoginAsync(
        string login,
        string password,
        CancellationToken cancellationToken = default);

    Task<OneOf<int, CustomException>> LogoutAsync(
        string? token,
        CancellationToken cancellationToken = default);

    IUser GetUser();

    bool IsAuthorized();
}