using EventosVivos.Domain.Exceptions;

namespace EventosVivos.Domain.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; init; }

        public Money(decimal amount)
        {
            if (amount < 0)
                throw new DomainRuleValidationException("The price cannot be negative.");

            Amount = amount;
        }
    }
}
