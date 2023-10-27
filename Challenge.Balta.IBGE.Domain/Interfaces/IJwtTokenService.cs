namespace Chanllenge.Balta.IBGE.Domain.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(string email);
    }
}
