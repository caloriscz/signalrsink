using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRSink;

namespace SerilogSignalRSink;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;

public class SignalRLoggerProvider : ILoggerProvider
{
    private readonly Func<IHubContext<MessageHub, IMessageHubClient>> _hubContextFactory;

    public SignalRLoggerProvider(Func<IHubContext<MessageHub, IMessageHubClient>> hubContextFactory)
    {
        _hubContextFactory = hubContextFactory;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new SignalRLogger(_hubContextFactory, categoryName);
    }

    public void Dispose()
    {
        // Dispose resources if necessary
    }
}