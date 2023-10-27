using Microsoft.AspNetCore.Identity;

namespace Chanllenge.Balta.IBGE.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(string username, string password);
        Task<SignInResult> LoginUserAsync(string username, string password);
    }
}
