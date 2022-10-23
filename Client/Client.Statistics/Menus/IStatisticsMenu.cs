namespace Client.Statistics.Menus;

public interface IStatisticsMenu
{
    Task StartAsync(CancellationToken cancellationToken);
}