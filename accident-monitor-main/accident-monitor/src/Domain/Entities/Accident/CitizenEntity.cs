using System.ComponentModel.DataAnnotations;

namespace AccidentMonitor.Domain.Entities.Accident;
public class CitizenEntity : BaseAuditableEntity
{
    public CitizenEntity(
        string citizenIdentityNumber, string fullName, DateOnly dateOfBirth, bool isMale, string nationality,
        string placeOfOrigin, string placeOfResidence, string verifiedPhoneNumber
        )
    {
        CitizenIdentityNumber = citizenIdentityNumber;
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        IsMale = isMale;
        Nationality = nationality;
        PlaceOfOrigin = placeOfOrigin;
        PlaceOfResidence = placeOfResidence;
        VerifiedPhoneNumber = verifiedPhoneNumber;
    }

    public string CitizenIdentityNumber { get; set; }
    public string FullName { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public bool IsMale { get; set; }
    public string Nationality { get; set; }
    public string PlaceOfOrigin { get; set; }
    public string PlaceOfResidence { get; set; }
    [Phone]
    public string VerifiedPhoneNumber { get; set; }
    public virtual ICollection<AccidentInvolved> AccidentsInvolved { get; set; } = [];
    public virtual ICollection<VehicleEntity> Vehicles { get; set; } = [];
}
