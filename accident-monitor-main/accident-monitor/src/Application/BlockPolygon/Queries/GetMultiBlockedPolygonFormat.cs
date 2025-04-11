using AccidentMonitor.Application.BlockPolygon.Mappers;
using AccidentMonitor.Application.Common.Interfaces;
using AccidentMonitor.Application.ORService.Dto;

namespace AccidentMonitor.Application.BlockPolygon.Queries;
public record GetMultiBlockedPolygonFormat : IRequest<AvoidPolygonsDto>;

public class GetMultiBlockedPolygonFormatHandler(
    IBlockedPolygonCoordRepository polygonCoordinateRepository)
    : IRequestHandler<GetMultiBlockedPolygonFormat, AvoidPolygonsDto>
{
    private readonly IBlockedPolygonCoordRepository _polygonCoordinateRepository
        = polygonCoordinateRepository;

    public async Task<AvoidPolygonsDto> Handle(GetMultiBlockedPolygonFormat request,
        CancellationToken cancellationToken)
    {
        var blockedPolygons = await _polygonCoordinateRepository.GetAllUnResolvedAsync();

        var blockedPolygonCoordinates = blockedPolygons
            .GroupBy(p => p.AccidentId)
            .Select(PolygonMapper.MapToMultiBlockedPolygon).ToList();

        return new AvoidPolygonsDto
        (
            Coordinates: blockedPolygonCoordinates
                .Select(p => p.Select(l => l.Select(c => new CoordinateDto(c.Longitude, c.Latitude)).ToList()).ToList())
                .ToList(),
            Type: "MultiPolygon"
        );
    }
}
