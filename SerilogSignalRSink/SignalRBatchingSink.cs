using Microsoft.AspNetCore.SignalR;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using SignalRSink;

namespace SerilogSignalRSink;

public class SignalRBatchingSink : PeriodicBatchingSink
{
    private readonly IHubContext<MessageHub, IMessageHubClient> _hubContext;

    public SignalRBatchingSink(
        IHubContext<MessageHub, IMessageHubClient> hubContext,
        int batchSizeLimit,
        TimeSpan period)
        : base(batchSizeLimit, period)
    {
        _hubContext = hubContext;
    }

    protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
    {
        foreach (var logEvent in events)
        {
            var message = logEvent.RenderMessage();
            var liveStats = new LiveStats
            {
                DateNow = DateTime.UtcNow,
                Message = message,
                Namespace = logEvent.Properties.TryGetValue("SourceContext", out var sourceContext)
                            ? sourceContext.ToString()
                            : "Unknown"
            };
            await _hubContext.Clients.All.SendStatistics(liveStats);
        }
    }
}
