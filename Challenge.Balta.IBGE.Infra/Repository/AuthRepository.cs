using Chanllenge.Balta.IBGE.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Challenge.Balta.IBGE.Infra.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthRepository(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SignInResult> LoginUserAsync(string username, string password)
        {
            try
            {
                return await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
            }
            catch (Exception)
            {
                throw new Exception("An internal error has occurred");
            }
        }

        public async Task<IdentityResult> RegisterUserAsync(string username, string password)
        {
            try
            {
                var user = new IdentityUser { UserName = username, Email = username };
                return await _userManager.CreateAsync(user, password);
            }
            catch (Exception)
            {
                throw new Exception("An internal error has occurred");
            }
        }
    }
}
