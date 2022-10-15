namespace Client.Account.Services;

public interface IUser
{
    public string? SessionId { get; }

    public string? Login { get; }
    
    bool IsAuthorized => !string.IsNullOrEmpty(SessionId) && !string.IsNullOrEmpty(Login);

    string GetBearerToken();
}