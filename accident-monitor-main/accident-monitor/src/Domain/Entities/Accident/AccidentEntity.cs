using AccidentMonitor.Domain.Entities.MapStuff.Polygons;

namespace AccidentMonitor.Domain.Entities.Accident
{
    public class AccidentEntity : BaseAuditableEntity
    {
        public AccidentEntity()
        {

        }
        public AccidentEntity(
            DateTime timestamp,
            float longitude,
            float latitude,
            bool isBlockingWay = false,
            AccidentSeverity severity = AccidentSeverity.Low,
            AccidentResolvedStatus resolvedStatus = AccidentResolvedStatus.Unresolved)
        {
            Timestamp = timestamp;
            Longitude = longitude;
            Latitude = latitude;
            IsBlockingWay = isBlockingWay;
            Severity = severity;
            ResolvedStatus = resolvedStatus;
        }

        public DateTime Timestamp { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public AccidentSeverity Severity { get; set; }
        public bool IsBlockingWay { get; set; }
        public AccidentResolvedStatus ResolvedStatus { get; set; }

        public virtual ICollection<AccidentInvolved> AccidentInvolved { get; set; } = [];

        public virtual ICollection<PolygonCoordinate> BlockedPolygonCoordinates { get; set; } = [];
    }
}
