using EventosVivos.Domain.Enums;
using EventosVivos.Domain.Exceptions;
using EventosVivos.Domain.Services;
using EventosVivos.Domain.ValueObjects;

namespace EventosVivos.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public Guid VenueId { get; private set; }
        public int MaxCapacity { get; private set; }
        public int AvailableTickets { get; private set; }
        public TimeFrame Schedule { get; private set; } = null!;
        public Money Price { get; private set; } = null!;
        public EventType Type { get; private set; }
        
        private EventStatus _status;

        // RN-06: Status is calculated on-the-fly if the end time has passed
        public EventStatus CurrentStatus
        {
            get
            {
                if (_status == EventStatus.Active && DateTime.UtcNow > Schedule.EndTime)
                    return EventStatus.Completed;
                return _status;
            }
        }

        private Event() { }

        public static async Task<Event> CreateAsync(
            string title, string description, Guid venueId, int venueCapacity,
            int requestedCapacity, TimeFrame schedule, Money price, EventType type,
            VenueAvailabilityChecker availabilityChecker)
        {
            ValidateInvariants(title, description, venueCapacity, requestedCapacity, schedule);

            // RN-02: Validate overlap using the Domain Service
            await availabilityChecker.EnsureIsAvailableAsync(venueId, schedule);

            return new Event
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                VenueId = venueId,
                MaxCapacity = requestedCapacity,
                AvailableTickets = requestedCapacity,
                Schedule = schedule,
                Price = price,
                Type = type,
                _status = EventStatus.Active
            };
        }

        private static void ValidateInvariants(string title, string description, int venueCapacity, int requestedCapacity, TimeFrame schedule)
        {
            if (title.Length < 5 || title.Length > 100)
                throw new DomainRuleValidationException("The title must be between 5 and 100 characters.");

            if (description.Length < 10 || description.Length > 500)
                throw new DomainRuleValidationException("The description must be between 10 and 500 characters.");

            if (requestedCapacity > venueCapacity)
                throw new DomainRuleValidationException("RN-01: The capacity cannot exceed the venue's capacity.");

            if (schedule.StartTime <= DateTime.UtcNow)
                throw new DomainRuleValidationException("The start date must be in the future.");
        }

        public void ReserveTickets(int quantity)
        {
            if (CurrentStatus != EventStatus.Active)
                throw new DomainRuleValidationException("Cannot reserve tickets for an inactive or completed event.");

            if (quantity < 1)
                throw new DomainRuleValidationException("You must reserve at least 1 ticket.");

            if (quantity > AvailableTickets)
                throw new DomainRuleValidationException("Capacity exceeded: Not enough tickets available.");

            AvailableTickets -= quantity;
        }

        public void ReleaseTickets(int quantity)
        {
            AvailableTickets += quantity;
            
            if (AvailableTickets > MaxCapacity)
                AvailableTickets = MaxCapacity;
        }

        public void CancelEvent()
        {
            _status = EventStatus.Cancelled;
        }
    }
}
