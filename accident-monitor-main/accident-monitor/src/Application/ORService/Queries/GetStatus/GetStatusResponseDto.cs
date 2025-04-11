using System.Text.Json.Serialization;
using AccidentMonitor.Application.ORService;

namespace AccidentMonitor.Application.ServicesCheck.ORService
{
    public class GetStatusORSResponseDto
    {
        [JsonPropertyName("engine")]
        public EngineInfoDto Engine { get; set; } = new EngineInfoDto();
        [JsonPropertyName("languages")]
        public string[]? Languages { get; set; }
        [JsonPropertyName("profiles")]
        public ProfileDto? Profiles { get; set; }
        [JsonPropertyName("services")]
        public string[]? Services { get; set; }
    }

    public class ProfileDto
    {
        [JsonPropertyName("driving-car")]
        public DrivingCarDto DrivingCar { get; set; } = new DrivingCarDto();
    }

    public class DrivingCarDto
    {
        [JsonPropertyName("storages")]
        public StoragesDto Storages { get; set; } = new StoragesDto();
        [JsonPropertyName("encoder_name")]
        public string EncoderName { get; set; } = string.Empty;
        [JsonPropertyName("encoded_values")]
        public string[] EncodedValues { get; set; } = [];
        [JsonPropertyName("graph_build_date")]
        public string GraphBuildDate { get; set; } = string.Empty;
        [JsonPropertyName("osm_date")]
        public string OsmDate { get; set; } = string.Empty;
        [JsonPropertyName("limits")]
        public LimitsDto Limits { get; set; } = new LimitsDto();
    }

    public class StoragesDto
    {
        [JsonPropertyName("Tollways")]
        public StorageDetailDto Tollways { get; set; } = new StorageDetailDto();
        [JsonPropertyName("WayCategory")]
        public StorageDetailDto WayCategory { get; set; } = new StorageDetailDto();
        [JsonPropertyName("HeavyVehicle")]
        public HeavyVehicleDto HeavyVehicle { get; set; } = new HeavyVehicleDto();
        [JsonPropertyName("WaySurfaceType")]
        public StorageDetailDto WaySurfaceType { get; set; } = new StorageDetailDto();
        [JsonPropertyName("RoadAccessRestrictions")]
        public RoadAccessRestrictionsDto RoadAccessRestrictions { get; set; } = new RoadAccessRestrictionsDto();
    }

    public class StorageDetailDto
    {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }

    public class HeavyVehicleDto
    {
        [JsonPropertyName("restrictions")]
        public bool Restrictions { get; set; }
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }

    public class RoadAccessRestrictionsDto
    {
        [JsonPropertyName("useForWarnings")]
        public bool UseForWarnings { get; set; }
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }

    public class LimitsDto
    {
        [JsonPropertyName("maximum_distance")]
        public int MaximumDistance { get; set; }
        [JsonPropertyName("maximum_waypoints")]
        public int MaximumWaypoints { get; set; }
        [JsonPropertyName("maximum_distance_dynamic_weights")]
        public int MaximumDistanceDynamicWeights { get; set; }
        [JsonPropertyName("maximum_distance_avoid_areas")]
        public int MaximumDistanceAvoidAreas { get; set; }
    }
}

