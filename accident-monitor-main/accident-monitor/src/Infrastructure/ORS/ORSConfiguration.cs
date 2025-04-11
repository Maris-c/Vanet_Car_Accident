namespace AccidentMonitor.Infrastructure.ORS;
public class ORSConfiguration
{
    public required Uri Uri { get; set; }
    public required int Port { get; set; }
    public required string BasePath { get; set; }
    public string ApiKey { get; set; } = string.Empty;
}
