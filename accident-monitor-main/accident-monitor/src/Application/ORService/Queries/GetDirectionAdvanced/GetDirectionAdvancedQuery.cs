using AccidentMonitor.Application.BlockPolygon.Mappers;
using AccidentMonitor.Application.Common.Interfaces;
using AccidentMonitor.Application.ORService.Dto;
using AccidentMonitor.Application.ORService.ExtensionMappers;
using AccidentMonitor.Application.ORService.Queries.GetDirectionAdvanced.Dtos;

namespace AccidentMonitor.Application.ORService.Queries.GetDirectionAdvanced;
public record GetDirectionAdvancedQuery (string Profile, GetDirectionAdvanceRequestDto RequestDto)
    : IRequest<DirectionCutResponseDto>;

public class GetDirectionAdvancedQueryHandler(IORService orServices,
        IBlockedPolygonCoordRepository blockedPolygonCoordRepository)
    : IRequestHandler<GetDirectionAdvancedQuery, DirectionCutResponseDto>
{
    private readonly IORService _orServices = orServices;
    private readonly IBlockedPolygonCoordRepository _blockedPolygonCoordRepository = blockedPolygonCoordRepository;
    public async Task<DirectionCutResponseDto> Handle(
        GetDirectionAdvancedQuery request,
        CancellationToken cancellationToken)
    {
        var blockedPolygonsCoordinate = await _blockedPolygonCoordRepository.GetAllUnResolvedAsync();
        var blockedPolygonCoordinates = blockedPolygonsCoordinate
            .GroupBy(p => p.AccidentId)
            .Select(PolygonMapper.MapToMultiBlockedPolygon).ToList();

        var blockedPolygon = new AvoidPolygonsDto
        (
            Coordinates: blockedPolygonCoordinates
                .Select(p => p.Select(l => l.Select(c => new CoordinateDto(c.Longitude, c.Latitude)).ToList()).ToList())
                .ToList(),
            Type: "MultiPolygon"
        );

        request.RequestDto.RoutingOptions ??= new GetDirectionAdvanceRequestDto.ORSDirectionAvoidOptionsDto();
        request.RequestDto.RoutingOptions.AvoidPolygons = blockedPolygon;

        var response = await _orServices.GetAdvancedRoutingDirectionAsync<GetDirectionAdvancedResponseDto>(
            request.Profile, request.RequestDto);

        return response.ToDirectionCutResponseDto();
    }

}
