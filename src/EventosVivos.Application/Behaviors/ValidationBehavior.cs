using EventosVivos.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventosVivos.Application.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    {
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
        private readonly ICorrelationContext _correlation;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger, ICorrelationContext correlation, IEnumerable<IValidator<TRequest>> validators)
        {
            _logger = logger;
            _correlation = correlation;
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var correlationId = _correlation.CorrelationId;

            _logger.LogInformation(
                "Validation started for {RequestName} | CorrelationId: {CorrelationId}",
                requestName,
                correlationId);

            if (!_validators.Any())
            {
                _logger.LogInformation(
                    "No validators found for {RequestName} | CorrelationId: {CorrelationId}",
                    requestName,
                    correlationId);

                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var results = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = results
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                _logger.LogWarning(
                    "Validation failed for {RequestName} | CorrelationId: {CorrelationId} | Errors: {ErrorCount}",
                    requestName,
                    correlationId,
                    failures.Count);

                foreach (var failure in failures)
                {
                    _logger.LogWarning(
                        "Validation error | CorrelationId: {CorrelationId} | {Property} - {Error}",
                        correlationId,
                        failure.PropertyName,
                        failure.ErrorMessage);
                }

                throw new ValidationException(failures);
            }

            _logger.LogInformation(
                "Validation succeeded for {RequestName} | CorrelationId: {CorrelationId}",
                requestName,
                correlationId);

            return await next();
        }
    }
}
