namespace EventosVivos.Application.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(string Username, string Role);
    }
}
