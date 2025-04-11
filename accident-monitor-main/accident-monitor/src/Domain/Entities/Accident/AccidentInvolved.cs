namespace AccidentMonitor.Domain.Entities.Accident;
public class AccidentInvolved : BaseAuditableEntity
{
    public AccidentInvolved() { }
    public AccidentInvolved(
        Guid accidentId,
        Guid vehicleId,
        Guid driverInvolvedId
    )
    {
        AccidentId = accidentId;
        VehicleId = vehicleId;
        DriverCitizenId = driverInvolvedId;
    }
    public Guid AccidentId { get; set; }
    public Guid DriverCitizenId { get; set; }
    public Guid VehicleId { get; set; }
    public virtual AccidentEntity Accident { get; set; } = null!;
    public virtual CitizenEntity DriverInvolved { get; set; } = null!;
    public virtual VehicleEntity VehicleInvolved { get; set; } = null!;
}
