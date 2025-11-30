using Car_rental_management_system.DTOs;
using Car_rental_management_system.ViewModel;
using Car_rental_system.Enum;
using Car_rental_system.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Car_rental_management_system.Controllers
{
    public static class IdentityUserEndpoint
    {
        public static IEndpointRouteBuilder MapIdentityUserEndpoint(
            this IEndpointRouteBuilder app,
            IConfiguration config)
        {
            app.MapPost("/SignUp", CreateUser);
            app.MapPost("/UserLogin", (UserManager<Users> um, LoginModel m)
                => LoginUser(um, m, config));

            return app;
        }

        [AllowAnonymous]
        private static async Task<IResult> CreateUser(
     UserManager<Users> userManager,
     [FromBody] RegisterationModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName))
                return Results.BadRequest("Username is required");
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model);

            if (!Validator.TryValidateObject(model, context, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }



            var user = new Users
            {
                Email = model.Email,
                UserName = model.UserName,
                DateOfBirth = model.DateOfBirth,
                gender = model.gender,
                LicenseNumber = model.LicenseNumber,
                ExpiryDate = model.ExpiryDate,
                LicenseType= model.LicenseType,
                PhoneNumber = model.PhoneNumber,
                City = model.City,
                Country = model.Country,
                Role=model.Role,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {


                return Results.Ok(new
                {
                    message = "User created successfully",
                    userId = user.Id
                });
            }

            return Results.BadRequest(new
            {
                errors = result.Errors.Select(e => e.Description)
            });
        }
        private static async Task<IResult> LoginUser(
            UserManager<Users> userManager,
            [FromBody] LoginModel userLogin,
            IConfiguration config)
        {

            var user = await userManager.FindByEmailAsync(userLogin.Email);

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(userLogin);

            if (!Validator.TryValidateObject(userLogin, context, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }
            if (user != null && await userManager.CheckPasswordAsync(user, userLogin.Password))
            {
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["JwtKey"]!)
                );

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("UserID", user.Id.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(
                        key,
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

                return Results.Ok(new
                {
                    token,
                    message = "تم تسجيل الدخول بنجاح",
                    user = new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        email = user.Email
                    }
                });
            }

            return Results.BadRequest(new { message = "خطأ فى تسجيل الدخول" });
        }
    }
}

