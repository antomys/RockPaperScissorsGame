using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Bll.Options;
using Server.Bll.Services.Interfaces;

namespace Server.Bll.Services;

public sealed class CleanerBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceProvider;
    private readonly ILogger<CleanerBackgroundService> _logger;
    private readonly PeriodicTimer _periodicTimer;
    private readonly CleanerOptions _cleanerOptions;

    public CleanerBackgroundService(
        ILogger<CleanerBackgroundService> logger, 
        IServiceScopeFactory serviceProvider,
        IOptions<CleanerOptions> cleanerOptions)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _cleanerOptions = cleanerOptions?.Value ?? throw new ArgumentNullException(nameof(cleanerOptions));
        _periodicTimer = new PeriodicTimer(_cleanerOptions.CleanPeriod);
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
        var rooms = await roomService
            .RemoveRangeAsync(_cleanerOptions.RoomOutDateTime, _cleanerOptions.RoundOutDateTime);

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