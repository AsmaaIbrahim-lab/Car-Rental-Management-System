using Car_rental_system.Data;
using Car_rental_system.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

namespace Car_rental_management_system.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityHandlersAndStores(this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<Users>()
              .AddEntityFrameworkStores<ApplicationDbContext>();
            return services;
        }
        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;

            });
            return services;

        }
        public static IServiceCollection AddAuthentication(this IServiceCollection services,IConfiguration config)
        {

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme =
                x.DefaultChallengeScheme =
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(y =>
            {
                y.SaveToken = true;
                y.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["JwtKey"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = config["JwtIssuer"],
                    ValidAudience = config["JwtAudience"],
                   
                };
            });

            return services;

        }
        public static WebApplication AddIdentityMiddleware(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;

        }

    }
    
}
