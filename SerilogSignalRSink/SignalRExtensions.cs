using Microsoft.AspNetCore.SignalR;
using Serilog;
using Serilog.Configuration;

namespace SignalRSink;

public static class SignalRSinkExtensions
{
    public static LoggerConfiguration SignalRSink(
            this LoggerSinkConfiguration loggerConfiguration,
            IHubContext<MessageHub, IMessageHubClient> hubContext,
            string url,
            string hubName)
    {
        return loggerConfiguration.Sink(new SignalRSink(hubContext, url, hubName));
    }

    public static void ConfigureSignalRSink(
        LoggerConfiguration loggerConfiguration,
        IHubContext<MessageHub, IMessageHubClient> hubContext,
        SignalRSettings settings)
    {
        loggerConfiguration.WriteTo.SignalRSink(hubContext, settings.Url, settings.HubName);
    }
}
