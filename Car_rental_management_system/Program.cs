using Car_rental_management_system.Controllers;
using Car_rental_management_system.Extensions;
using Car_rental_system.Data;
using Car_rental_system.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Car_rental_management_system
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString).UseLazyLoadingProxies()) ;

            builder.Services.AddControllers();

            builder.Services.AddIdentityHandlersAndStores()
                           .ConfigureIdentityOptions()
                           .AddAuthentication(builder.Configuration); 

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Fill in the JWT token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.AddIdentityMiddleware(); 

            app.configCORS(builder.Configuration);

            app.MapIdentityApi<Users>();
            app.MapGroup("/api")
                .MapIdentityUserEndpoint(builder.Configuration);
            //  .MapAccountEndpoints();
            
            app.MapControllers();

            app.Run();
        }
    }
}