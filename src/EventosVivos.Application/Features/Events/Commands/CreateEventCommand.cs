using EventosVivos.Application.Interfaces;
using EventosVivos.Domain.Entities;
using EventosVivos.Domain.Enums;
using EventosVivos.Domain.Ports;
using EventosVivos.Domain.Services;
using EventosVivos.Domain.ValueObjects;
using FluentValidation;
using MediatR;

namespace EventosVivos.Application.Features.Events.Commands
{
    public record CreateEventCommand(
        string Title,
        string Description,
        Guid VenueId,
        int MaxCapacity,
        DateTime StartTime,
        DateTime EndTime,
        decimal Price,
        EventType EventType) : IRequest<Guid>;

    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().Length(5, 100);
            RuleFor(x => x.Description).NotEmpty().Length(10, 500);
            RuleFor(x => x.MaxCapacity).GreaterThan(0);
            RuleFor(x => x.StartTime).GreaterThan(DateTime.UtcNow).WithMessage("La fecha de inicio debe ser futura.");
            RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime).WithMessage("La fecha de fin debe ser posterior al inicio.");
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.EventType).IsInEnum();
        }
    }

    public class CreateEventCommandHandler(
        IEventRepository eventRepository,
        IVenueRepository venueRepository,
        VenueAvailabilityChecker availabilityChecker,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateEventCommand, Guid>
    {
        public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var schedule = new TimeFrame(request.StartTime, request.EndTime);
            var price = new Money(request.Price);

            var venueCapacity = await venueRepository.GetVenueCapacityAsync(request.VenueId);

            // La entidad valida RN-01, RN-02 y RN-03 internamente
            var newEvent = await Event.CreateAsync(
                request.Title, request.Description, request.VenueId, venueCapacity.HasValue ? venueCapacity.Value : 0,
                request.MaxCapacity, schedule, price, request.EventType, availabilityChecker);

            await eventRepository.AddAsync(newEvent);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return newEvent.Id;
        }
    }
}
