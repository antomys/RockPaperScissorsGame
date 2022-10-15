namespace Client.StartMenu.Services;

public interface IHealthCheckService
{
    Task ConnectAsync();

    Task PingAsync();
}