namespace Client.Account.Services;

internal sealed class User : IUser
{
    public static readonly User Default = new(string.Empty, string.Empty);
    
    internal User(string sessionId, string login)
    {
        SessionId = sessionId;
        Login = login;
    }
    
    public string SessionId { get; }

    public string Login { get; }
    
    
    public bool IsAuthorized => !string.IsNullOrEmpty(SessionId) && !string.IsNullOrEmpty(Login);
    
   
    public string GetBearerToken()
    {
        return $"Bearer {SessionId}";
    }
}