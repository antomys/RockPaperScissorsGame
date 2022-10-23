namespace Client.Account.Menus;

public interface IAccountMenu
{
    Task StartAsync(CancellationToken cancellationToken);
    
    Task<bool> LogoutAsync(CancellationToken cancellationToken);
}