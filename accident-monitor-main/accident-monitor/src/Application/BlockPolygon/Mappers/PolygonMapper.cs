using AccidentMonitor.Application.BlockPolygon.Dtos;
using AccidentMonitor.Domain.Entities.MapStuff;
using AccidentMonitor.Domain.Entities.MapStuff.Polygons;

namespace AccidentMonitor.Application.BlockPolygon.Mappers;
public static class PolygonMapper
{
    /// <summary>
    /// Convert a group of PolygonCoordinates into a list of lists of Coordinates.
    /// Each group represents a polygon with a ring.
    /// </summary>
    /// <param name="group">Group of PolygonCoordinates</param>
    /// <returns>List of lists of Coordinates representing a MultiPolygon</returns>
    public static List<List<CoordinateEntity>> MapToMultiBlockedPolygon(IGrouping<Guid, PolygonCoordinate> group)
    {
        var coordinates = group.Select(p => p.Coordinate).ToList();
        if (coordinates.Count > 0 &&
            (coordinates.First().Longitude != coordinates.Last().Longitude ||
                coordinates.First().Latitude != coordinates.Last().Latitude))
        {
            coordinates.Add(coordinates.First());
        }
        return [coordinates];
    }

    /// <summary>
    /// Convert a group of PolygonCoordinates into a BlockedPolygonDto.
    /// Each group represents a polygon with a ring.
    /// </summary>
    /// <param name="group">Group of PolygonCoordinates</param>
    /// <returns>BlockedPolygonDto with the AccidentId and list of Coordinates</returns>
    public static BlockedPolygonDto MapToBlockedPolygonDto(IGrouping<Guid, PolygonCoordinate> group)
    {
        var coordinates = group.Select(p => p.Coordinate).ToList();
        if (coordinates.Count > 0 &&
            (coordinates.First().Longitude != coordinates.Last().Longitude ||
                coordinates.First().Latitude != coordinates.Last().Latitude))
        {
            coordinates.Add(coordinates.First());
        }
        return new BlockedPolygonDto
        {
            AccidentId = group.Key,
            Coordinates = coordinates
        };
    }
}
