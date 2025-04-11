namespace AccidentMonitor.Application.ORService.Queries.GetDirections;
public class GetDirectionQueryValidator : AbstractValidator<GetDirectionQuery>
{
    public GetDirectionQueryValidator()
    {
        RuleFor(x => x.Profile)
            .NotEmpty().WithMessage("Origin is required.");
        RuleFor(x => x.RequestDto.DestinationCoordinate)
            .NotEmpty().WithMessage("Destination is required.");
        RuleFor(x => x.RequestDto.StartingCoordinate)
            .NotEmpty().WithMessage("Destination is required.");
    }
}

