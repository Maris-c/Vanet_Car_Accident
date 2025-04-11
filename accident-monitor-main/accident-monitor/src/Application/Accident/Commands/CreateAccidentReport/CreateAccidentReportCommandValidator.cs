namespace AccidentMonitor.Application.Accident.Commands.CreateAccidentReport;
public class CreateAccidentReportCommandValidator : AbstractValidator<CreateAccidentOnReportCommand>
{
    public CreateAccidentReportCommandValidator()
    {
        RuleFor(x => x.Time)
            .NotEmpty()
            .WithMessage("Time is required.");
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90 degrees.");
        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180 degrees.");
        RuleFor(x => x.Severity)
            .NotEmpty()
            .IsInEnum()
            .WithMessage("Severity must be a valid enum value.");
    }
}
