using RockPaperScissors.Common.Client;
using RockPaperScissors.Common.Extensions;

namespace Client.StartMenu.Services;

internal sealed class HealthCheckService : IHealthCheckService
{
    private readonly IClient _client;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public HealthCheckService(IClient client, CancellationTokenSource cancellationTokenSource)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _cancellationTokenSource =
            cancellationTokenSource ?? throw new ArgumentNullException(nameof(cancellationTokenSource));
    }

    public async Task ConnectAsync()
    {
        "\nTrying to connect to the server...".Print(ConsoleColor.White);
        var result = await _client.GetAsync("/health", _cancellationTokenSource.Token);

        if (result)
        {
            "Connected to the server".Print(ConsoleColor.Green);

            return;
        }

        "Failed to connect. Closing... \nPress any key...".Print(ConsoleColor.Red);
        _cancellationTokenSource.Cancel();
    }
    
    public async Task PingAsync()
    {
        await Task.Factory.StartNew(async () =>
        {
            "Starting health checks".Print(ConsoleColor.White);
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var result = await _client.GetAsync("/health", _cancellationTokenSource.Token);

                if (!result)
                {
                    "\nConnection to the server lost. Stopping...\nPress any key...".Print(ConsoleColor.Red);
                
                    _cancellationTokenSource.Cancel();
                }

                await Task.Delay(1000);
            }
        }, TaskCreationOptions.LongRunning);
    }
}