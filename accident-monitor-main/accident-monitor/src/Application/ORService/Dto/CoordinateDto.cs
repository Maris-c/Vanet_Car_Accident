namespace AccidentMonitor.Application.ORService.Dto;
public class CoordinateDto
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public CoordinateDto(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }
    public CoordinateDto()
    {
        Longitude = 0;
        Latitude = 0;
    }
    public override string ToString()
    {
        return $"[{Longitude}, {Latitude}]";
    }
}
