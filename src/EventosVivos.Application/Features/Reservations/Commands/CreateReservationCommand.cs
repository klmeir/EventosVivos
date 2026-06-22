using EventosVivos.Application.Interfaces;
using EventosVivos.Domain.Entities;
using EventosVivos.Domain.Ports;
using EventosVivos.Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace EventosVivos.Application.Features.Reservations.Commands
{
    public record CreateReservationCommand(
    Guid EventId,
    int Quantity,
    string BuyerName,
    string BuyerEmail) : IRequest<Guid>;

    public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationCommandValidator()
        {
            RuleFor(x => x.EventId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(1);
            RuleFor(x => x.BuyerName).NotEmpty();
            RuleFor(x => x.BuyerEmail).NotEmpty().EmailAddress();
        }
    }

    public class CreateReservationCommandHandler(
        IEventRepository eventRepository,
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateReservationCommand, Guid>
    {
        public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var eventObj = await eventRepository.GetByIdAsync(request.EventId)
                ?? throw new KeyNotFoundException("No associated event found.");

            var email = new EmailAddress(request.BuyerEmail);
            
            var reservation = Reservation.Create(eventObj, request.Quantity, request.BuyerName, email, DateTime.UtcNow);

            eventObj.ReserveTickets(request.Quantity);

            await reservationRepository.AddAsync(reservation);
            await eventRepository.UpdateAsync(eventObj);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return reservation.Id;
        }
    }
}
