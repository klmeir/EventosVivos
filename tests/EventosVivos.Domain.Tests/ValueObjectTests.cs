using EventosVivos.Domain.Exceptions;
using EventosVivos.Domain.ValueObjects;
using FluentAssertions;

namespace EventosVivos.Domain.Tests
{
    public class ValueObjectTests
    {
        [Fact]
        public void EmailAddress_ShouldThrowException_WhenFormatIsInvalid()
        {
            var act1 = () => new EmailAddress("correo-sin-arroba.com");
            var act2 = () => new EmailAddress("");

            act1.Should().Throw<DomainRuleValidationException>();
            act2.Should().Throw<DomainRuleValidationException>();
        }

        [Fact]
        public void Money_ShouldThrowException_WhenAmountIsNegative()
        {
            var act = () => new Money(-50);
            act.Should().Throw<DomainRuleValidationException>();
        }

        [Fact]
        public void TimeFrame_ShouldThrowException_WhenWeekendAndLate_RN03()
        {
            // Arrange: Un sábado a las 22:30 (Hora local convertida a un DateTime de fin de semana)
            var saturday = new DateTime(2026, 6, 20, 22, 30, 0);
            var end = saturday.AddHours(2);

            // Act
            var act = () => new TimeFrame(saturday, end);

            // Assert
            act.Should().Throw<DomainRuleValidationException>()
               .WithMessage("*RN-03*");
        }

        [Fact]
        public void TimeFrame_ShouldThrowException_WhenEndIsBeforeStart()
        {
            var start = DateTime.UtcNow.AddDays(1);
            var act = () => new TimeFrame(start, start.AddHours(-1));

            act.Should().Throw<DomainRuleValidationException>();
        }
    }
}
