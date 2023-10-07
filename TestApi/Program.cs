using Microsoft.AspNetCore.SignalR;
using Serilog;
using SerilogSignalRSink;
using SignalRSink;
using TestApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddSingleton<ILoggerProvider>(serviceProvider =>
    new SignalRLoggerProvider(() => serviceProvider.GetRequiredService<IHubContext<MessageHub, IMessageHubClient>>())
);
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation($"test");

    return "deep";
})
.WithName("GetMe");

app.MapGet("/weatherforecast", (ILogger<Program> logger) =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
    .ToArray();

    logger.LogInformation($"say hello to test ..................................");

    return forecast;
})
.WithName("GetWeatherForecast");

app.UseCors(x => x
    .SetIsOriginAllowed(origin => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.MapHub<MessageHub>("/live");
app.Logger.LogError("starting the app ...");
app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}