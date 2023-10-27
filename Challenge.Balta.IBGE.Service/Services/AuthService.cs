using Chanllenge.Balta.IBGE.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Challenge.Balta.IBGE.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<SignInResult> LoginUserAsync(string username, string password)
        {
            return await _authRepository.LoginUserAsync(username, password);
        }

        public async Task<IdentityResult> RegisterUserAsync(string username, string password)
        {
            return await _authRepository.RegisterUserAsync(username, password);   
        }
    }
}
