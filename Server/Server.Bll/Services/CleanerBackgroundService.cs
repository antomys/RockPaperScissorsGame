using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Bll.Services.Interfaces;

namespace Server.Bll.Services;

public sealed class CleanerBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceProvider;
    private readonly ILogger<CleanerBackgroundService> _logger;
    private readonly PeriodicTimer _periodicTimer;
    // todo: options of max time

    public CleanerBackgroundService(
        ILogger<CleanerBackgroundService> logger, 
        IServiceScopeFactory serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(10));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Background service");

        while (await _periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            await CleanJunk(_serviceProvider);
        }
    }

    private async Task CleanJunk(IServiceScopeFactory factory)
    {
        using var scope = factory.CreateScope();
        var roomService = scope.ServiceProvider.GetRequiredService<IRoomService>();
        //todo: timespan to option.
        var rooms = await roomService
            .RemoveRangeAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(20));

        if (rooms > 0)
        {
            _logger.LogInformation("Cleaned {Room} entities", rooms.ToString());
        }
    }

    public override void Dispose()
    {
        _periodicTimer.Dispose();
        base.Dispose();
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _periodicTimer.Dispose();
        
        return base.StopAsync(cancellationToken);
    }
}