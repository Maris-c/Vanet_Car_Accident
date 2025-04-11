namespace AccidentMonitor.Domain.Entities.Accident
{
    public class VehicleEntity(string registrationCertificateNumber, string licensePlate, string engineNumber,
                         string chassisNumber, string type, string brand, string model, string vehicleOwnerName,
                         string address, Guid ownerId) : BaseAuditableEntity
    {
        public string RegistrationCertificateNumber { get; set; } = registrationCertificateNumber;
        public string LicensePlate { get; set; } = licensePlate;
        public string EngineNumber { get; set; } = engineNumber;
        public string ChassisNumber { get; set; } = chassisNumber;
        public string Type { get; set; } = type;
        public string Brand { get; set; } = brand;
        public string Model { get; set; } = model;
        public string VehicleOwnerName { get; set; } = vehicleOwnerName;
        public string Address { get; set; } = address;
        public Guid OwnerId { get; set; } = ownerId;
        public virtual ICollection<AccidentInvolved> AccidentInvolved { get; set; } = [];
        public virtual CitizenEntity Owner { get; set; } = null!;
    }
}
