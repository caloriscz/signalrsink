using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRSink;

namespace SerilogSignalRSink;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;

public class SignalRLogger : ILogger
{
    private readonly Func<IHubContext<MessageHub, IMessageHubClient>> _hubContextFactory;
    private readonly string _categoryName;

    public SignalRLogger(Func<IHubContext<MessageHub, IMessageHubClient>> hubContextFactory, string categoryName)
    {
        _hubContextFactory = hubContextFactory;
        _categoryName = categoryName;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var hubContext = _hubContextFactory();  // Get the IHubContext instance here
        var message = formatter(state, exception);
        var liveStats = new LiveStats
        {
            DateNow = DateTime.Now,
            Message = message,
            Namespace = _categoryName
        };
        hubContext.Clients.All.SendStatistics(liveStats);
    }
}