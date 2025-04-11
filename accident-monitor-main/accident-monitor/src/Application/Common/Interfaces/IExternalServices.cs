namespace AccidentMonitor.Application.Common.Interfaces;
public interface IExternalServices
{
    Task<TResponse> HealthCheck<TResponse>();
    Task<TResponse> GetStatus<TResponse>();
}
