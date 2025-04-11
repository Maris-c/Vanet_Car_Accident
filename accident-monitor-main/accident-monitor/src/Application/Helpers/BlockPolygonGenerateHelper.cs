using AccidentMonitor.Domain.Entities.Accident;
using AccidentMonitor.Domain.Entities.MapStuff;
using AccidentMonitor.Domain.Entities.MapStuff.Polygons;

namespace AccidentMonitor.Application.Helpers
{
    public static class BlockPolygonGenerateHelper
    {
        /// <summary>
        /// Generates a square polygon's coordinates based on the source accident's location.
        /// </summary>
        /// <param name="source">The source accident containing the base longitude and latitude.</param>
        /// <param name="accident">The AccidentEntity to which the generated coordinates will be assigned.</param>
        /// <param name="size">The offset size to determine the square's dimensions.</param>
        /// <returns>A list of BlockPolygon that define the square polygon.</returns>
        public static ICollection<PolygonCoordinate> GenerateSquareCoordinates(AccidentEntity source, float size = 0.00005F)
        {
            var baseLongitude = source.Longitude;
            var baseLatitude = source.Latitude;
            var id = source.Guid;

            var topRight = new PolygonCoordinate { AccidentId = id, Coordinate = new CoordinateEntity(baseLongitude + size, baseLatitude + size) };
            var bottomRight = new PolygonCoordinate { AccidentId = id, Coordinate = new CoordinateEntity(baseLongitude + size, baseLatitude - size) };
            var bottomLeft = new PolygonCoordinate { AccidentId = id, Coordinate = new CoordinateEntity(baseLongitude - size, baseLatitude - size) };
            var topLeft = new PolygonCoordinate { AccidentId = id, Coordinate = new CoordinateEntity(baseLongitude - size, baseLatitude + size) };

            return [topRight, bottomRight, bottomLeft, topLeft];
        }
    }
}
