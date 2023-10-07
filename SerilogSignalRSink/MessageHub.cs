using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace SignalRSink;

public interface IMessageHubClient
{
    Task SendStatistics(LiveStats stats);
}

public class MessageHub : Hub<IMessageHubClient>
{
    private Timer _timer;
    private readonly ILogger _logger;

    public MessageHub(ILogger<MessageHub> logger)
    {
        _timer = new Timer(SendRandomStatistics, null, 0, 150000);
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogError("We are there");

        LiveStats stats = new LiveStats { DateNow = DateTime.Now, Namespace = "", Message = $"Hello {Context.ConnectionId} Welcome to Tadata Stream 😏" };

        await Clients.Caller.SendStatistics(stats);

        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        _timer?.Dispose();
        Console.WriteLine("Connection closed: " + Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendStatistics(LiveStats stats)
    {
        await Clients.All.SendStatistics(stats);
    }

    private void SendRandomStatistics(object state)
    {
        try
        {
            if (Clients == null)
            {
                _timer?.Dispose();
                return;
            }

            string randomString = Guid.NewGuid().ToString();
            LiveStats stats = new LiveStats
            {
                DateNow = DateTime.Now,
                Namespace = "",
                Message = randomString
            };
            Clients.All.SendStatistics(stats);
        }
        catch (Exception e)
        {
            _logger.LogError($"Cannot send information: {e}");
            throw;
        }
    }
}
