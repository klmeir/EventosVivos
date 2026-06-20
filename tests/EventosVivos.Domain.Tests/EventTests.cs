using EventosVivos.Domain.Entities;
using EventosVivos.Domain.Enums;
using EventosVivos.Domain.Exceptions;
using EventosVivos.Domain.Ports;
using EventosVivos.Domain.Services;
using EventosVivos.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace EventosVivos.Domain.Tests
{
    public class EventTests
    {
        private readonly Mock<IEventRepository> _eventRepoMock = new();
        private readonly VenueAvailabilityChecker _availabilityChecker;

        public EventTests()
        {
            _availabilityChecker = new VenueAvailabilityChecker(_eventRepoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenVenueIsOccupied_RN02()
        {
            // Arrange
            var schedule = new TimeFrame(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1).AddHours(2));

            // Simulamos que el venue está ocupado
            _eventRepoMock.Setup(r => r.HasOverlappingEventsAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                          .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await Event.CreateAsync("Evento Test", "Descripcion larga necesaria", Guid.NewGuid(), 100, 50,
                schedule, new Money(50), EventType.Conference, _availabilityChecker);

            // Assert
            await act.Should().ThrowAsync<DomainRuleValidationException>()
                     .WithMessage("*RN-02*");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenRequestedCapacityExceedsVenue_RN01()
        {
            // Arrange
            var schedule = new TimeFrame(DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(2).AddHours(2));
            int venueCapacity = 100;
            int requestedCapacity = 150; // Excede la capacidad

            // Simulamos que el venue está libre
            _eventRepoMock.Setup(r => r.HasOverlappingEventsAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                          .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await Event.CreateAsync("Evento Test", "Descripcion larga necesaria", Guid.NewGuid(),
                venueCapacity, requestedCapacity, schedule, new Money(50), EventType.Conference, _availabilityChecker);

            // Assert
            await act.Should().ThrowAsync<DomainRuleValidationException>()
                     .WithMessage("*RN-01*");
        }

        [Fact]
        public void ReserveTickets_ShouldDecreaseAvailableTickets_WhenValid()
        {
            // Arrange
            var eventObj = CreateTestEvent(DateTime.UtcNow.AddDays(5), 100, 50m);

            // Act
            eventObj.ReserveTickets(5);

            // Assert
            eventObj.AvailableTickets.Should().Be(95);
        }

        [Fact]
        public void ReserveTickets_ShouldThrowException_WhenExceedingCapacity()
        {
            // Arrange
            var eventObj = CreateTestEvent(DateTime.UtcNow.AddDays(5), 10, 50m);

            // Act
            var act = () => eventObj.ReserveTickets(15);

            // Assert
            act.Should().Throw<DomainRuleValidationException>()
               .WithMessage("*Not enough tickets available*");
        }

        [Fact]
        public void ReleaseTickets_ShouldNotExceedMaxCapacity()
        {
            // Arrange
            var eventObj = CreateTestEvent(DateTime.UtcNow.AddDays(5), 100, 50m);
            eventObj.ReserveTickets(10); // Quedan 90

            // Act
            eventObj.ReleaseTickets(20); // Intentamos devolver de más

            // Assert
            eventObj.AvailableTickets.Should().Be(100);
        }

        [Fact]
        public void CurrentStatus_ShouldBeCompleted_WhenEndTimeHasPassed_RN06()
        {
            // Arrange
            var pastStart = DateTime.UtcNow.AddDays(-2);
            var eventObj = CreateTestEvent(pastStart, 100, 50m);

            // Act
            var status = eventObj.CurrentStatus;

            // Assert
            status.Should().Be(EventStatus.Completed);
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
            eventType.GetField("_status", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                     ?.SetValue(eventObj, EventStatus.Active);
            return eventObj;
        }
    }
}
