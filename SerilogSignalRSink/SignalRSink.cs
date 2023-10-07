using Microsoft.AspNetCore.SignalR;
using Serilog.Core;
using Serilog.Events;

namespace SignalRSink;

public class SignalRSink : ILogEventSink
{
    private readonly IHubContext<MessageHub, IMessageHubClient> _hubContext;
    private readonly string _url;
    private readonly string _hubName;

    public SignalRSink(IHubContext<MessageHub, IMessageHubClient> hubContext, string url, string hubName)
    {
        _hubContext = hubContext;
        _url = url;
        _hubName = hubName;
    }

    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage();
        var namespaceContent = logEvent.Properties.ContainsKey("SourceContext") ? logEvent.Properties["SourceContext"].ToString() : "Unknown";

        var liveStats = new LiveStats
        {
            DateNow = DateTime.Now,
            Message = message,
            Namespace = namespaceContent
        };

        _hubContext.Clients.All.SendStatistics(liveStats);
    }
}
