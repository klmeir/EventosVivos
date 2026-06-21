using EventosVivos.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EventosVivos.Api.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainRuleValidationException ex)
            {
                await WriteProblemDetails(
                    context,
                    StatusCodes.Status422UnprocessableEntity,
                    ex.Message,
                    ex);
            }
            catch (KeyNotFoundException ex)
            {
                await WriteProblemDetails(
                    context,
                    StatusCodes.Status404NotFound,
                    ex.Message,
                    ex);
            }
            catch (ValidationException ex)
            {
                await WriteValidationProblemDetails(
                    context,
                    StatusCodes.Status400BadRequest,
                    ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception");

                await WriteProblemDetails(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred",
                    ex);
            }
        }

        private async Task WriteProblemDetails(
            HttpContext context,
            int statusCode,
            string title,
            Exception ex)
        {
            var correlationId =
                context.Response.Headers["X-Correlation-Id"]
                    .FirstOrDefault()
                ?? context.TraceIdentifier;

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = $"https://httpstatuses.com/{statusCode}",
                Instance = context.Request.Path
            };

            problem.Extensions["correlationId"] =
                correlationId;

            if (_environment.IsDevelopment())
            {
                problem.Detail = ex.ToString();
            }
            else
            {
                problem.Detail =
                    $"An error occurred. CorrelationId: {correlationId}";
            }

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(problem);
        }

        private async Task WriteValidationProblemDetails(
            HttpContext context,
            int statusCode,
            ValidationException ex)
        {
            var correlationId =
                context.Response.Headers["X-Correlation-Id"]
                    .FirstOrDefault()
                ?? context.TraceIdentifier;

            var errors = ex.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray());

            var problem = new ValidationProblemDetails(errors)
            {
                Status = statusCode,
                Title = "Validation failed",
                Type = $"https://httpstatuses.com/{statusCode}",
                Instance = context.Request.Path
            };

            problem.Extensions["correlationId"] =
                correlationId;

            if (_environment.IsDevelopment())
            {
                problem.Detail = ex.ToString();
            }

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
