using EventosVivos.Application.Features.Reservations.Commands;
using EventosVivos.Application.Features.Reservations.Queries;
using MediatR;

namespace EventosVivos.Api.Endpoints
{
    public static class ReservationEndpoints
    {
        public static RouteGroupBuilder MapReservationEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (CreateReservationCommand command, ISender mediator) =>
                Results.Ok(await mediator.Send(command)));
            
            group.MapGet("/event/{eventId:guid}", async (Guid eventId, ISender mediator) =>
                Results.Ok(await mediator.Send(new GetReservationsByEventQuery(eventId))));

            group.MapPost("/{id:guid}/confirm", async (Guid id, ISender mediator) =>
                Results.Ok(await mediator.Send(new ConfirmPaymentCommand(id))))
                .RequireAuthorization("AdminOnly");

            group.MapPost("/{id:guid}/cancel", async (Guid id, ISender mediator) =>
                Results.Ok(await mediator.Send(new CancelReservationCommand(id))));

            return group;
        }
    }
}
