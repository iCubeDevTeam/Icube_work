using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Data;
using Server.Interfaces;
using Server.Services;

namespace Server.Extensions
{
    public static class ApplicationServiceExtensions
    {
         public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config )
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(config.GetConnectionString("ServerContext")));

            return services;
        }
    }
}