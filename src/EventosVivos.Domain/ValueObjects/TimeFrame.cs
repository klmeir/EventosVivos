using EventosVivos.Domain.Exceptions;

namespace EventosVivos.Domain.ValueObjects
{
    public record TimeFrame
    {
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }

        public TimeFrame(DateTime startTime, DateTime endTime)
        {
            if (endTime <= startTime)
                throw new DomainRuleValidationException("The end date must be after the start date.");

            // RN-03: Weekend night schedule restriction
            bool isWeekend = startTime.DayOfWeek == DayOfWeek.Saturday || startTime.DayOfWeek == DayOfWeek.Sunday;
            if (isWeekend && startTime.Hour >= 22)
            {
                throw new DomainRuleValidationException("RN-03: Weekend events cannot start after 22:00.");
            }

            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
