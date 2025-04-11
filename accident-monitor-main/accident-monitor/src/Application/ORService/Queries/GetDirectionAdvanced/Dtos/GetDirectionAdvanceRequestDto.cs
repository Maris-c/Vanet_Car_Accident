using System.Text.Json.Serialization;
using AccidentMonitor.Application.Converter;
using AccidentMonitor.Application.ORService.Dto;

namespace AccidentMonitor.Application.ORService.Queries.GetDirectionAdvanced.Dtos;

/// <summary>
/// Represents the advanced direction request DTO that contains routing information including coordinates, language,
/// options for routing such as polygon avoidance, measurement units, and geometry flags.
/// </summary>
public class GetDirectionAdvanceRequestDto
{
    /// <summary>
    /// Collection of waypoints that are desired intermediate points on the route.
    /// [
    ///     {longitude1, latitude1}, // this one is the starting point
    ///     {longitude2, latitude2}, // this one is a waypoint
    ///     ...
    ///     {longitudeN, latitudeN} // this one is the destination
    /// ]
    /// </summary>
    [JsonPropertyName("coordinates")]
    public List<double[]> WaypointCoordinates { get; set; } = [];


    /// <summary>
    /// The flag indicating whether to perform geometry simplification.
    /// The value is expected as a string "true" or "false".
    /// </summary>
    [JsonPropertyName("geometry_simplify")]
    public bool GeometrySimplify { get; set; } = false;


    /// <summary>
    /// The flag indicating whether to return the geometry of the route.
    /// The value is expected as a string "true" or "false".
    /// </summary>
    [JsonPropertyName("geometry")]
    public bool Geometry { get; set; } = false;

    /// <summary>
    /// The language of the route instructions. 
    /// Current available languages are:
    ///    ["cs", "da", "de"],
    ///    ["en", "eo", "es"],
    ///    ["fi", "fr", "gr"],
    ///    ["he", "hu", "id"],
    ///    ["it", "ja", "nb"],
    ///    ["ne", "nl", "pl"],
    ///    ["pt", "ro", "ru"],
    ///    ["tr", "ua", "vi"],
    ///    ["zh"]
    /// </summary>
    [JsonPropertyName("language")]
    public string Language { get; set; } = "en";


    /// <summary>
    /// This is the routing options.With the request body parameter options, 
    /// advanced routing options can be specified for a directions request.
    /// Some available options are:
    ///     - avoid_polygons: Avoid certain polygons which using the GeoJson format. 
    ///         Learn more here https://datatracker.ietf.org/doc/html/rfc7946#appendix-A.6 
    ///     - avoid_features: Avoid certain features 
    ///         ["highways", "tollways", "ferries", "fords", "steps"]
    ///         (NOT IMPLEMENTED YET)
    ///     - avoid_borders: Avoid crossing borders. 
    ///         (NOT IMPLEMENTED YET)
    ///     - avoid_countries: Avoid certain countries. 
    ///         Which can be found here https://giscience.github.io/openrouteservice/technical-details/country-list
    ///         (NOT IMPLEMENTED YET)
    ///     
    /// </summary>
    [JsonPropertyName("options")]
    public ORSDirectionAvoidOptionsDto? RoutingOptions { get; set; } = new ORSDirectionAvoidOptionsDto();

    /// <summary>
    /// Represents the routing options for the advanced directions request.
    /// Currently supports options such as avoiding specific polygons defined in GeoJson format.
    /// </summary>
    public class ORSDirectionAvoidOptionsDto
    {
        /// <summary>
        /// The polygon avoidance options defined in the GeoJson format.
        /// </summary>
        [JsonPropertyName("avoid_polygons")]
        public AvoidPolygonsDto? AvoidPolygons { get; set; } 
        //[JsonPropertyName("avoid_features")]
        //public string[]? AvoidFeatures { get; set; } = [];
        //[JsonPropertyName("avoid_borders")]
        //public int[]? AvoidCountries { get; set; } = [];
    }

    /// <summary>
    /// The unit of measurement for distances.
    /// Some available units are:
    ///     - "m" for meters
    ///     - "km" for kilometers
    ///     - "mi" for miles
    /// 
    /// </summary>
    [JsonPropertyName("units")]
    public string? Units { get; set; } = "m";

    [JsonPropertyName("preference")]
    public string? Preference = "recommended";

    [JsonPropertyName("instructions")]
    public bool? Instructions = true;

    [JsonPropertyName("instructions_format")]
    public string? InstructionFormat = "text";
}
