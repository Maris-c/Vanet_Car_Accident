using AccidentMonitor.Application.Common.Exceptions;
using AccidentMonitor.Application.Common.Interfaces;
namespace AccidentMonitor.Application.ORService.Queries.HealthCheck;

public record HealthCheckQuery : IRequest<HealthCheckResponseDto>;
public class HealthCheckQueryHandler(IORService orServices) : IRequestHandler<HealthCheckQuery, HealthCheckResponseDto>
{
    private readonly IORService _orServices = orServices;

    public async Task<HealthCheckResponseDto> Handle(HealthCheckQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _orServices.HealthCheck<HealthCheckResponseDto>();
        }
        catch (ServicesUnavailableException)
        {
            return new HealthCheckResponseDto
            {
                Message = "Open route sevice is not ready.",
                Status = "unhealthy"
            };
        }
        catch (HttpRequestException)
        {
            return new HealthCheckResponseDto
            {
                Status = "unhealthy",
                Message = "Failed to establish connection with ORS."
            };
        }
        catch (InternalServerErrorException)
        {
            return new HealthCheckResponseDto
            {
                Status = "unhealthy",
                Message = "An internal error has occured."
            };
        }
    }
}
