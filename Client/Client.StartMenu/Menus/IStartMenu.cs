namespace Client.StartMenu.Menus;

public interface IStartMenu
{
    Task PrintAsync(CancellationToken cancellationToken);
}