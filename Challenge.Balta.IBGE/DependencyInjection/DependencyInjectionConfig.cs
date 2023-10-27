using Challenge.Balta.IBGE.Domain.Interfaces;
using Challenge.Balta.IBGE.FluentValidator;
using Challenge.Balta.IBGE.Infra.Data.Context;
using Challenge.Balta.IBGE.Infra.Repository;
using Challenge.Balta.IBGE.MapperProfile;
using Challenge.Balta.IBGE.Model;
using Challenge.Balta.IBGE.Service.Services;
using Chanllenge.Balta.IBGE.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Balta.IBGE.DependencyInjection
{
    public static class DependencyInjectionConfig
    {
        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connetionString = configuration.GetConnectionString("DefaultConnection");

            // Configurações do DbContext
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connetionString));

            // Registro de serviços
            services.AddScoped<ILocalityService, LocalityService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthService, AuthService>();

            // Configuração de Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<SignInManager<IdentityUser>>();
            services.AddScoped<UserManager<IdentityUser>>();

            // Registro do repositorio
            services.AddScoped<IFileImportRepository, FileImportRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<ILocalityRepository, LocalityRepository>();

            // AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //Validator
            services.AddScoped<IValidator<CreateUserModel>, CreateUserModelValidator>();
            services.AddScoped<IValidator<LoginModel>, LoginModelValidator>();
            services.AddScoped<IValidator<IbgeModel>, IbgeModelValidator>();
            services.AddScoped<IValidator<IFormFile>, ExcelFileValidator>();
        }
    }
}
