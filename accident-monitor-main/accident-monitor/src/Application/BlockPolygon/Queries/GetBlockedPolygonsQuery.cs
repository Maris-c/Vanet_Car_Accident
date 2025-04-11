using AccidentMonitor.Application.BlockPolygon.Dtos;
using AccidentMonitor.Application.BlockPolygon.Mappers;
using AccidentMonitor.Application.Common.Interfaces;

namespace AccidentMonitor.Application.BlockPolygon.Queries;
public record GetBlockedPolygonsQuery : IRequest<List<BlockedPolygonDto>>;

public class GetBlockedPolygonQueryHandler(
    IBlockedPolygonCoordRepository polygonCoordinateRepository)
    : IRequestHandler<GetBlockedPolygonsQuery, List<BlockedPolygonDto>>
{
    private readonly IBlockedPolygonCoordRepository _polygonCoordinateRepository = polygonCoordinateRepository;

    public async Task<List<BlockedPolygonDto>> Handle(GetBlockedPolygonsQuery request,
        CancellationToken cancellationToken)
    {
        var blockedPolygons = await _polygonCoordinateRepository.GetAllAsync();
        var blockedPolygonCoordinates = blockedPolygons
            .GroupBy(p => p.AccidentId)
            .Select(PolygonMapper.MapToBlockedPolygonDto).ToList();

        return blockedPolygonCoordinates;
    }
}
