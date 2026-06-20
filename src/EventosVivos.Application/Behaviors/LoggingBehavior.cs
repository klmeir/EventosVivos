using EventosVivos.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EventosVivos.Application.Behaviors
{
    public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly ICorrelationContext _correlation;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, ICorrelationContext correlation)
        {
            _logger = logger;
            _correlation = correlation;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {

            var requestName = typeof(TRequest).Name;
            var correlationId = _correlation.CorrelationId;

            var stopwatch = Stopwatch.StartNew();

            try
            {
                _logger.LogInformation(
                    "Handling {RequestName} | CorrelationId: {CorrelationId}",
                    requestName,
                    correlationId);

                var response = await next();

                stopwatch.Stop();

                _logger.LogInformation(
                    "Completed {RequestName} in {DurationMs}ms | CorrelationId: {CorrelationId}",
                    requestName,
                    stopwatch.ElapsedMilliseconds,
                    correlationId);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(
                    ex,
                    "Failed {RequestName} after {DurationMs}ms | CorrelationId: {CorrelationId}",
                    requestName,
                    stopwatch.ElapsedMilliseconds,
                    correlationId);

                throw;
            }
        }
    }
}
