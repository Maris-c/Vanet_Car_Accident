
using AccidentMonitor.Application.Accident.Commands.CreateAccidentReport;
using AccidentMonitor.Application.Accident.Commands.UpdateResolveStatus;
using AccidentMonitor.Domain.Entities.Accident;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AccidentMonitor.Web.Endpoints;

public class AccidentReport : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            //.RequireAuthorization()
            .MapPost(ReportAccident, "/report-accident")
            .MapPut(UpdateAccidentStatus, "/update-resolve");
    }

    public async Task<Created<Guid>> ReportAccident(ISender sender, CreateAccidentOnReportCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(AccidentEntity)}/{id}", id);
    }

    public async Task<Results<NoContent, BadRequest<string>>> UpdateAccidentStatus(
        ISender sender,
        Guid accidentId,
        UpdateAccidentResolvedStatusCommand command)
    {
        var updatedCommand = command with { AccidentId = accidentId, IsResolved = true };
        var id = await sender.Send(updatedCommand);

        return id == Guid.Empty
            ? TypedResults.BadRequest("Accident not found.")
            : TypedResults.NoContent();
    }
}
