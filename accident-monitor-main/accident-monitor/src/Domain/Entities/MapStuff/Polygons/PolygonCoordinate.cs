using System.Text.Json.Serialization;
using AccidentMonitor.Domain.Entities.Accident;

namespace AccidentMonitor.Domain.Entities.MapStuff.Polygons
{
    public class PolygonCoordinate : BaseEntity, IEquatable<PolygonCoordinate>
    {
        public PolygonCoordinate() { }

        public PolygonCoordinate(float longitude, float latitude)
        {
            Coordinate = new CoordinateEntity(longitude, latitude);
        }

        public Guid AccidentId { get; set; }
        [JsonIgnore]
        public virtual AccidentEntity Accident { get; set; } = null!;

        public CoordinateEntity Coordinate { get; set; } = new CoordinateEntity();

        public bool Equals(PolygonCoordinate? other)
        {
            return other is not null
                && Coordinate.Longitude == other.Coordinate.Longitude
                && Coordinate.Latitude == other.Coordinate.Latitude;
        }

        public override int GetHashCode()
            => HashCode.Combine(Coordinate.Longitude, Coordinate.Latitude);

        public override bool Equals(object? obj)
            => Equals(obj as PolygonCoordinate);
    }
}
