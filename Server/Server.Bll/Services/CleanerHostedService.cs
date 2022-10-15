using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Bll.Services.Interfaces;

namespace Server.Bll.Services;

public sealed class CleanerHostedService : IHostedService
{
    private readonly IServiceScopeFactory _serviceProvider;
    private readonly ILogger<CleanerHostedService> _logger;
    private Timer? _timer;
        
    // todo: options of max time

    public CleanerHostedService(
        ILogger<CleanerHostedService> logger, 
        IServiceScopeFactory serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Cleaning");
        
        _timer = new Timer(
            CleanJunk, 
            _serviceProvider,
            TimeSpan.FromSeconds(10), 
            TimeSpan.FromSeconds(10));
        
        return Task.CompletedTask;
    }

    private async void CleanJunk(object? state)
    {
        var factory = (IServiceScopeFactory) state!;
        using var scope = factory.CreateScope();
        var roomService = scope.ServiceProvider.GetRequiredService<IRoomService>();
        //todo: timespan to option.
        var rooms = await roomService
            .RemoveRangeAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(20));
        
        if(rooms > 0)
            _logger.LogInformation("Cleaned {Room} entities", rooms.ToString());
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
            
        return Task.CompletedTask;
    }
}