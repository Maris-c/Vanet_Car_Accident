using AccidentMonitor.Application.ORService.Dto;
using AccidentMonitor.Application.ORService.Queries.GetDirectionAdvanced;
using AccidentMonitor.Application.ORService.Queries.GetDirectionAdvanced.Dtos;
using AccidentMonitor.Application.ORService.Queries.GetDirections;
using AccidentMonitor.Application.ORService.Queries.GetDirections.Dtos;
using AccidentMonitor.Application.ORService.Queries.GetStatus;
using AccidentMonitor.Application.ORService.Queries.HealthCheck;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AccidentMonitor.Web.Endpoints;
public class RoutingDirection : EndpointGroupBase
{

    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this);
        //.RequireAuthorization();
        group.MapGet("status", GetStatus);
        group.MapGet("health", HealthCheck);
        group.MapGet("direction/{profile}", GetDirection);
        group.MapPost("direction/{profile}", GetDirectionAdvanced);
    }

    private async Task<IResult> HealthCheck(ISender sender)
    {
        var result = await sender.Send(new HealthCheckQuery());
        return result.Status == "ready" ? TypedResults.Ok(result) : TypedResults.InternalServerError(result);
    }

    public async Task<IResult> GetStatus(ISender sender)
    {
        var result = await sender.Send(new GetStatusQuery());

        return TypedResults.Ok(result);
    }

    public async Task<Results<Ok<DirectionCutResponseDto>, BadRequest>> GetDirection(
        ISender sender, [FromRoute] string profile,
        [FromQuery] string start, [FromQuery] string end)
    {
        var requestDto = new GetDirectionRequestDto
        {
            StartingCoordinate = start,
            DestinationCoordinate = end
        };
        var query = new GetDirectionQuery(profile, requestDto);
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public async Task<Results<Ok<DirectionCutResponseDto>, BadRequest>> GetDirectionAdvanced(
        ISender sender, [FromRoute] string profile,
        [FromBody] GetDirectionAdvanceRequestDto requestDto)
    {
        var query = new GetDirectionAdvancedQuery(profile, requestDto);
        var result = await sender.Send(query);
        return TypedResults.Ok(result);
    }
}
