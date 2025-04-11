using AccidentMonitor.Application.Common.Results;

namespace AccidentMonitor.Application.Common.Interfaces;

public interface IMqttService : IExternalServices
{
    Task<ServiceResult> StartAsync();
    Task<ServiceResult> ConnectAsync();
    ValueTask DisposeAsync();
    Task<ServiceResult> PublishAsync<T>(string topic, T dto);
    Task<ServiceResult> SubscribeAsync();
}
