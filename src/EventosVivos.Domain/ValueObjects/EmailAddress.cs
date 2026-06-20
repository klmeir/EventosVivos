using EventosVivos.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace EventosVivos.Domain.ValueObjects
{
    public record EmailAddress
    {
        public string Value { get; init; }

        public EmailAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new DomainRuleValidationException("Invalid email format.");

            Value = value;
        }
    }
}
