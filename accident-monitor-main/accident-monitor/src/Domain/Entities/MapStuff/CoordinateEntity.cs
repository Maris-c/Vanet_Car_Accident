namespace AccidentMonitor.Domain.Entities.MapStuff;
public class CoordinateEntity
{
    public CoordinateEntity()
    {
    }

    public CoordinateEntity(CoordinateEntity other)
    {
        Latitude = other.Latitude;
        Longitude = other.Longitude;
    }
    public CoordinateEntity(float longitude, float latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}
