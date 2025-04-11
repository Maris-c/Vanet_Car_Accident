using AccidentMonitor.Application.Common.Interfaces;
using AccidentMonitor.Application.ORService.Dto;
using AccidentMonitor.Application.ORService.ExtensionMappers;
using AccidentMonitor.Application.ORService.Queries.GetDirections.Dtos;

namespace AccidentMonitor.Application.ORService.Queries.GetDirections
{
    public record GetDirectionQuery(string Profile, GetDirectionRequestDto RequestDto)
        : IRequest<DirectionCutResponseDto>;

    public class GetDirectionQueryHandler(IORService orServices)
        : IRequestHandler<GetDirectionQuery, DirectionCutResponseDto>
    {
        private readonly IORService _orServices = orServices;

        public async Task<DirectionCutResponseDto>
            Handle(GetDirectionQuery request, CancellationToken cancellationToken)
        {
            var response = await _orServices.GetRoutingDirectionAsync<GetDirectionResponseDto>(
                request.Profile, request.RequestDto);
            return response.ToDirectionCutResponseDto();
        }
    }
}
