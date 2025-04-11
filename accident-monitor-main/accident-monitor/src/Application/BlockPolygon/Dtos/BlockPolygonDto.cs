using System.Text.Json.Serialization;
using AccidentMonitor.Domain.Entities.MapStuff;

namespace AccidentMonitor.Application.BlockPolygon.Dtos
{
    /// <summary>
    /// Represents the blocked polygon information with the AccidentId and list of coordinates.
    /// Each coordinate is represented in the format [longitude, latitude].
    /// </summary>
    public class BlockedPolygonDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the accident.
        /// </summary>
        [JsonPropertyName("AccidentId")]
        public Guid AccidentId { get; set; }

        /// <summary>
        /// Gets or sets the list of coordinates.
        /// Each coordinate is represented as an array [longitude, latitude].
        /// </summary>
        [JsonPropertyName("Coordinates")]
        public List<CoordinateEntity> Coordinates { get; set; } = [];
    }
}
