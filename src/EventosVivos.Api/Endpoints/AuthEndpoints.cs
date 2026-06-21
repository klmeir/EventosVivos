using EventosVivos.Application.Interfaces;

namespace EventosVivos.Api.Endpoints
{
    public record LoginRequest(string Username, string Password);
    public record LoginResponse(string Token);

    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/login", (LoginRequest request, IConfiguration config, IAuthService auth) =>
            {
                if (request.Username == config["AdminCredentials:Username"] &&
                    request.Password == config["AdminCredentials:Password"])
                {
                    var token = auth.GenerateToken(request.Username, "admin");
                    return Results.Ok(new LoginResponse(token));
                }
                return Results.Unauthorized();
            });
        }
    }
}
