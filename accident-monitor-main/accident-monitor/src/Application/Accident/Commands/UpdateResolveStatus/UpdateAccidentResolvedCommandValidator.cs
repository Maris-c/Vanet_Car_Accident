namespace AccidentMonitor.Application.Accident.Commands.UpdateResolveStatus;
public class UpdateAccidentResolvedStatusCommandValidator : AbstractValidator<UpdateAccidentResolvedStatusCommand>
{
    public UpdateAccidentResolvedStatusCommandValidator()
    {
        RuleFor(x => x.AccidentId)
            .NotEmpty()
            .WithMessage("Accident ID is required.")
            .WithErrorCode("Bad request.");
        RuleFor(x => x.IsResolved)
            .NotNull()
            .WithMessage("Resolved status is required.")
            .WithErrorCode("Bad request.");
    }
}
