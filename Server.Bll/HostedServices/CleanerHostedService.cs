using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Bll.Services;
using Server.Bll.Services.Interfaces;

namespace Server.Bll.HostedServices
{
    public class CleanerHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceProvider;
        private readonly ILogger<CleanerHostedService> _logger;
        private Timer _timer;
        
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
            _timer = new Timer(
                CleanJunk, 
                _serviceProvider,
                TimeSpan.FromSeconds(10), 
                TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        private async void CleanJunk(object state)
        {
            _logger.LogInformation("Starting Cleaning.");
            var factory = (IServiceScopeFactory) state;
            using var scope = factory.CreateScope();
            var roomService = scope.ServiceProvider.GetRequiredService<IHostedRoomService>();

            var rooms = await roomService
                .RemoveEntityRangeByDate(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(20));
            
            _logger.LogInformation("Cleaned {0} entities",rooms);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }
    }
}