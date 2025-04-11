using AccidentMonitor.Domain.Entities.MapStuff.Polygons;

namespace AccidentMonitor.Application.Common.Interfaces;
public interface IBlockedPolygonCoordRepository : IRepository<PolygonCoordinate>
{
    Task<List<PolygonCoordinate>> DeleteResolvedPolygonCoordinatesAsync(Guid accidentId);
    Task<List<PolygonCoordinate>> GetAllUnResolvedAsync();
}
