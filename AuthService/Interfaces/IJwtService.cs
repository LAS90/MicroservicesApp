namespace AuthService.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string email);
    }
}
