using EventosVivos.Application.Interfaces;

namespace EventosVivos.Api.Common
{
    public sealed class CorrelationContext : ICorrelationContext
    {
        private readonly IHttpContextAccessor _http;

        public CorrelationContext(IHttpContextAccessor http)
        {
            _http = http;
        }

        public string CorrelationId =>
            _http.HttpContext?.Items["CorrelationId"]?.ToString()
            ?? "unknown";
    }
}
