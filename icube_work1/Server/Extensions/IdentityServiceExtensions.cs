using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Server.AuthorizationRequirements;

namespace Server.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config )
        {
           services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasTagService",policyBuilder =>{
                    policyBuilder.AddRequirements(new CustomRequireClaim("TagService"));
                });
                options.AddPolicy("HasInterfaceService",policyBuilder =>{
                    policyBuilder.AddRequirements(new CustomRequireClaim("InterfaceService"));
                });
                options.AddPolicy("HasIntegrationService",policyBuilder =>{
                    policyBuilder.AddRequirements(new CustomRequireClaim("IntegrationService"));
                });
            });
            services.AddScoped<IAuthorizationHandler,CustomRequireClaimHandler>();
            return services;
        }
    }
}