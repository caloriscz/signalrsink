using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using SignalRSink;

namespace TestApi;

public class SignalRLoggingSetupService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly IOptions<SignalRSettings> _signalRSettings;

    public SignalRLoggingSetupService(IServiceProvider serviceProvider, IConfiguration configuration, IOptions<SignalRSettings> signalRSettings)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;  // Set _configuration here
        _signalRSettings = signalRSettings;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<MessageHub, IMessageHubClient>>();
            var signalRSettings = _signalRSettings.Value;
            var loggerConfiguration = new LoggerConfiguration();
            SignalRSinkExtensions.ConfigureSignalRSink(loggerConfiguration, hubContext, signalRSettings);
            Log.Logger = loggerConfiguration.CreateLogger();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Log.CloseAndFlush();

        return Task.CompletedTask;
    }
}
