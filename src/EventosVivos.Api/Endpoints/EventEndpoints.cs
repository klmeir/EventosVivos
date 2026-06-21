using EventosVivos.Application.Features.Events.Commands;
using EventosVivos.Application.Features.Events.Queries;
using MediatR;

namespace EventosVivos.Api.Endpoints
{
    public static class EventEndpoints
    {
        public static RouteGroupBuilder MapEventEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (CreateEventCommand command, ISender mediator) =>
                Results.Ok(await mediator.Send(command)));

            group.MapGet("/", async ([AsParameters] GetEventsQuery query, ISender mediator) =>
                Results.Ok(await mediator.Send(query)));

            group.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
                Results.Ok(await mediator.Send(new GetEventByIdQuery(id))));

            group.MapGet("/{id:guid}/report", async (Guid id, ISender mediator) =>
                Results.Ok(await mediator.Send(new GetEventReportQuery(id))));

            return group;
        }
    }
}
