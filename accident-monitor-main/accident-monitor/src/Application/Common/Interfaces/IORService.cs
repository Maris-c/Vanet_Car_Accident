using AccidentMonitor.Application.ORService.Queries.GetDirectionAdvanced.Dtos;
using AccidentMonitor.Application.ORService.Queries.GetDirections.Dtos;

namespace AccidentMonitor.Application.Common.Interfaces;
public interface IORService : IExternalServices
{
    Task<TResponse> GetRoutingDirectionAsync<TResponse>(
        string profile, GetDirectionRequestDto request);
    Task<TResponse> GetAdvancedRoutingDirectionAsync<TResponse>(
        string profile, GetDirectionAdvanceRequestDto request);

}
