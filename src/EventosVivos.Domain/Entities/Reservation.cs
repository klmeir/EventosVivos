using EventosVivos.Domain.Enums;
using EventosVivos.Domain.Exceptions;
using EventosVivos.Domain.ValueObjects;

namespace EventosVivos.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; private set; }
        public Guid EventId { get; private set; }
        public string BuyerName { get; private set; } = string.Empty;
        public EmailAddress BuyerEmail { get; private set; } = null!;
        public int Quantity { get; private set; }
        public ReservationStatus Status { get; private set; }
        public string? Code { get; private set; }
        public int LostSeats { get; private set; } // To handle penalties RN-07
        public DateTime CreatedAt { get; private set; }

        private Reservation() { }

        // Reservation factory that validates business rules (RN-04 and RN-05)
        public static Reservation Create(
            Event targetEvent, int quantity, string buyerName,
            EmailAddress buyerEmail, DateTime currentDate)
        {                        
            var hoursUntilEvent = (targetEvent.Schedule.StartTime - currentDate).TotalHours;

            // RN-04: Late reservation restriction
            if (hoursUntilEvent < 1)
                throw new DomainRuleValidationException("RN-04: Reservations are not allowed less than 1 hour before the start.");

            // RF-03 Rule: Less than 24h, max 5 per transaction
            if (hoursUntilEvent < 24 && quantity > 5)
                throw new DomainRuleValidationException("Less than 24 hours before the event, you can only reserve a maximum of 5 tickets.");

            // RN-05: Premium events max 10 per transaction
            if (targetEvent.Price.Amount > 100 && quantity > 10)
                throw new DomainRuleValidationException("RN-05: For events priced over $100, the limit is 10 tickets per transaction.");

            return new Reservation
            {
                Id = Guid.NewGuid(),
                EventId = targetEvent.Id,
                BuyerName = buyerName,
                BuyerEmail = buyerEmail,
                Quantity = quantity,
                Status = ReservationStatus.PendingPayment,
                CreatedAt = currentDate
            };
        }

        public void ConfirmPayment()
        {
            if (Status == ReservationStatus.Confirmed)
                throw new DomainRuleValidationException("The reservation is already confirmed.");

            if (Status == ReservationStatus.Cancelled)
                throw new DomainRuleValidationException("Cannot confirm a cancelled reservation.");

            Status = ReservationStatus.Confirmed;
            // Generates random code EV-XXXXXX
            Code = $"EV-{new Random().Next(100000, 999999)}";
        }

        // Returns 'true' if the tickets should be made available again in the event
        public bool Cancel(DateTime eventStartTime, DateTime currentDate)
        {
            if (Status == ReservationStatus.Cancelled)
                throw new DomainRuleValidationException("The reservation was already cancelled.");

            bool releaseTickets = true;

            // RN-07: Cancellation with penalty if confirmed and less than 48h remaining
            if (Status == ReservationStatus.Confirmed)
            {
                var hoursUntilEvent = (eventStartTime - currentDate).TotalHours;
                if (hoursUntilEvent < 48)
                {
                    LostSeats = Quantity; // Seats are marked as lost
                    releaseTickets = false; // Tickets are NOT returned to the event pool
                }
            }

            Status = ReservationStatus.Cancelled;

            return releaseTickets;
        }
    }
}
