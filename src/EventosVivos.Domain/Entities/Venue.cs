using EventosVivos.Domain.Exceptions;

namespace EventosVivos.Domain.Entities
{
    public class Venue
    {        
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public string City { get; private set; }
        
        private Venue() { }

        public Venue(Guid id, string name, int capacity, string city)
        {
            if (capacity <= 0)
                throw new DomainRuleValidationException("Venue capacity must be greater than zero.");

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainRuleValidationException("Venue name is required.");

            Id = id;
            Name = name;
            Capacity = capacity;
            City = city;
        }
    }
}
