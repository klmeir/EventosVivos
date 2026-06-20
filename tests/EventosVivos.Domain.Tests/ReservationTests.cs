using EventosVivos.Domain.Entities;
using EventosVivos.Domain.Enums;
using EventosVivos.Domain.Exceptions;
using EventosVivos.Domain.ValueObjects;
using FluentAssertions;

namespace EventosVivos.Domain.Tests
{
    public class ReservationTests
    {
        [Fact]
        public void Create_ShouldThrowException_WhenReservingTooCloseToEvent_RN04()
        {
            // Arrange: Evento empieza en 30 minutos
            var start = DateTime.UtcNow.AddMinutes(30);
            var eventObj = CreateTestEvent(start, 100, 50m);

            // Act
            Action act = () => Reservation.Create(eventObj, 2, "Juan", new EmailAddress("test@test.com"), DateTime.UtcNow);

            // Assert
            act.Should().Throw<DomainRuleValidationException>()
               .WithMessage("*RN-04*");
        }

        [Fact]
        public void Create_ShouldThrowException_WhenUnder24HoursAndMoreThan5Tickets_RF03()
        {
            // Arrange: Faltan 10 horas
            var start = DateTime.UtcNow.AddHours(10);
            var eventObj = CreateTestEvent(start, 100, 50m);

            // Act
            var act = () => Reservation.Create(eventObj, 6, "Juan", new EmailAddress("test@test.com"), DateTime.UtcNow);

            // Assert
            act.Should().Throw<DomainRuleValidationException>()
               .WithMessage("*Less than 24 hours*");
        }

        [Fact]
        public void Create_ShouldThrowException_WhenPriceOver100AndMoreThan10Tickets_RN05()
        {
            // Arrange: Evento VIP (Precio $150)
            var start = DateTime.UtcNow.AddDays(10);
            var eventObj = CreateTestEvent(start, 100, 150m);

            // Act
            var act = () => Reservation.Create(eventObj, 11, "Juan", new EmailAddress("test@test.com"), DateTime.UtcNow);

            // Assert
            act.Should().Throw<DomainRuleValidationException>()
               .WithMessage("*RN-05*");
        }

        [Fact]
        public void ConfirmPayment_ShouldGenerateCodeAndChangeStatus()
        {
            // Arrange
            var eventObj = CreateTestEvent(DateTime.UtcNow.AddDays(5), 100, 50m);
            var res = Reservation.Create(eventObj, 2, "Juan", new EmailAddress("test@test.com"), DateTime.UtcNow);

            // Act
            res.ConfirmPayment();

            // Assert
            res.Status.Should().Be(ReservationStatus.Confirmed);
            res.Code.Should().NotBeNullOrEmpty();
            res.Code.Should().StartWith("EV-");
        }

        [Fact]
        public void Cancel_ShouldReturnTrueAndNoPenalty_WhenCancelledOver48Hours()
        {
            // Arrange: Faltan 5 días
            var eventStart = DateTime.UtcNow.AddDays(5);
            var eventObj = CreateTestEvent(eventStart, 100, 50m);
            var res = Reservation.Create(eventObj, 2, "Juan", new EmailAddress("test@test.com"), DateTime.UtcNow);
            res.ConfirmPayment();

            // Act
            bool shouldRelease = res.Cancel(eventStart, DateTime.UtcNow);

            // Assert
            shouldRelease.Should().BeTrue();
            res.LostSeats.Should().Be(0);
            res.Status.Should().Be(ReservationStatus.Cancelled);
        }

        [Fact]
        public void Cancel_ShouldPenalize_WhenCancelledUnder48Hours_RN07()
        {
            // Arrange
            var eventStart = DateTime.UtcNow.AddHours(24);
            var eventObj = CreateTestEvent(eventStart, 100, 50m);
            var res = Reservation.Create(eventObj, 2, "Juan", new EmailAddress("test@test.com"), eventStart.AddDays(-5));
            res.ConfirmPayment();

            // Act
            bool shouldRelease = res.Cancel(eventStart, DateTime.UtcNow);

            // Assert
            shouldRelease.Should().BeFalse();
            res.LostSeats.Should().Be(2);
            res.Status.Should().Be(ReservationStatus.Cancelled);
        }

        // Helper
        private static Event CreateTestEvent(DateTime start, int venueCap, decimal price)
        {
            var eventObj = (Event)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(Event));
            var eventType = typeof(Event);
            eventType.GetProperty("Id")?.SetValue(eventObj, Guid.NewGuid());
            eventType.GetProperty("Schedule")?.SetValue(eventObj, new TimeFrame(start, start.AddHours(2)));
            eventType.GetProperty("Price")?.SetValue(eventObj, new Money(price));
            eventType.GetProperty("MaxCapacity")?.SetValue(eventObj, venueCap);
            eventType.GetProperty("AvailableTickets")?.SetValue(eventObj, venueCap);
            return eventObj;
        }
    }
}
