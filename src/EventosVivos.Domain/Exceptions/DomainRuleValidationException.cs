namespace EventosVivos.Domain.Exceptions
{
    public class DomainRuleValidationException : Exception
    {
        public DomainRuleValidationException(string message) : base(message) { }
    }
}
