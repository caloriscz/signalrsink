using Microsoft.AspNetCore.SignalR;
using Serilog.Configuration;
using Serilog;
using SignalRSink;

namespace SerilogSignalRSink;

public static class LoggerConfigurationExtensions
{
    public static LoggerConfiguration WriteToSignalR(
        this LoggerSinkConfiguration sinkConfiguration,
        IHubContext<MessageHub, IMessageHubClient> hubContext,
        int batchSizeLimit = 1000,
        TimeSpan? period = null)
    {
        period ??= TimeSpan.FromSeconds(2);
        var batchingSink = new SignalRBatchingSink(hubContext, batchSizeLimit, period.Value);
        return sinkConfiguration.Sink(batchingSink);
    }
}