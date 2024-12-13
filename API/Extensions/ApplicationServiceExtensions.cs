using API.Data;
using API.Interfaces;
using API.Services;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}