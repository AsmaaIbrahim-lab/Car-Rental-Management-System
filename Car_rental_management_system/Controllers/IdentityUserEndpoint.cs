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
using System.Text.RegularExpressions;

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
            app.MapPost("/Logout", LogoutUser)
         .RequireAuthorization();

            return app;
        }

        [AllowAnonymous]
        private static async Task<IResult> CreateUser(
     UserManager<Users> userManager,
     [FromBody] RegisterationModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName))
                return Results.BadRequest("Username is required");

           
            if (model.ExpiryDate< DateOnly.FromDateTime(DateTime.Now))
            {
                return Results.BadRequest("The  License is expired");
            }
            if (string.IsNullOrWhiteSpace(model.LicenseNumber))
                return Results.BadRequest("License number is required.");

            if (model.LicenseNumber.Length != 8)
                return Results.BadRequest("License number must be exactly 8 characters.");

            var regex = new Regex(@"^[A-Z]{2}[0-9]{6}$");
            if (!regex.IsMatch(model.LicenseNumber))
                return Results.BadRequest("License number must be 2 uppercase letters followed by 6 digits.");

            if (string.IsNullOrWhiteSpace(model.Email))
                return Results.BadRequest("Email is required.");

            if (model.Email.Length > 80)
                return Results.BadRequest("Email cannot exceed 80 characters.");

            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(model.Email))
                return Results.BadRequest("Email format is invalid.");

            if (string.IsNullOrWhiteSpace(model.UserName))
                return Results.BadRequest("Username is required.");

            var RegEx = new Regex(@"^[a-zA-Z0-9]{3,20}$");

            if (!RegEx.IsMatch(model.UserName))
                return Results.BadRequest("Username must be 3-20 characters with letters and digits only.");

            if (model.DateOfBirth == null)
                return Results.BadRequest("Date of birth is required.");

            if (string.IsNullOrWhiteSpace(model.PhoneNumber))
                return Results.BadRequest("Phone number is required.");

            var reg = new Regex(@"^(010|011|012|015)[0-9]{8}$");
            if (!reg.IsMatch(model.PhoneNumber))
                return Results.BadRequest("Phone number must be a valid Egyptian number.");

            if (!System.Enum.IsDefined(typeof(City), model.City) || (int)model.City == 0)
                return Results.BadRequest("City is required and must be a valid value.");

            if (!System.Enum.IsDefined(typeof(Role), model.Role) || (int)model.Role == 0)
                return Results.BadRequest("Role is required and must be a valid value.");

            if (string.IsNullOrWhiteSpace(model.Country))
                return Results.BadRequest("Country is required.");

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
                        new Claim(ClaimTypes.Role, user.Role.ToString())


                    }),

                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = config["JwtIssuer"],      
                    Audience = config["JwtAudience"],
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
        [Authorize]
        public static async Task<IResult> LogoutUser(
    HttpContext context,
    SignInManager<Users> signInManager)
        {
            try
            {
                await signInManager.SignOutAsync();

                context.Response.Cookies.Delete(".AspNetCore.Identity.Application");

                return Results.Ok(new
                {
                    message = "تم تسجيل الخروج بنجاح",
                    success = true,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    title: "حدث خطأ أثناء تسجيل الخروج",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

    }
}

