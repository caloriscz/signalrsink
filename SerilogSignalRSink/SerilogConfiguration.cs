using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SignalRSink;

namespace SerilogSignalRSink;

public static class SerilogConfiguration
{
    public static LoggerConfiguration Configure(
        LoggerConfiguration config,
        IServiceProvider services)
    {
        var hubContext = services.GetRequiredService<IHubContext<MessageHub, IMessageHubClient>>();
        return config.WriteTo.SignalRSink(hubContext, "http://localhost:5080", "live");
    }
}