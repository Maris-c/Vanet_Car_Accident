using AccidentMonitor.Application.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AccidentMonitor.Infrastructure.MQTT;
public static class InitializerExtensions
{
    public static async Task InitializeMqttAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<MqttClientContextInitializer>();
        await initializer.InitializeAsync();

    }
}

public class MqttClientContextInitializer
{
    private readonly ILogger<MqttClientContextInitializer> _logger;
    private readonly IMqttService _mqttServices;

    public MqttClientContextInitializer(
        ILogger<MqttClientContextInitializer> logger,
        IMqttService mqttServices)
    {
        _logger = logger;
        _mqttServices = mqttServices;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _mqttServices.StartAsync();
            await _mqttServices.PublishAsync("server/HealthCheck", new
            {
                Status = "Ok"
            });
            //await _mqttServices.ConnectAsync();
            _logger.LogInformation("Broker connection initialized.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the broker connection.");
            throw;
        }
        //finally
        //{
        //    await _mqttServices.DisposeAsync();
        //}
    }
}

