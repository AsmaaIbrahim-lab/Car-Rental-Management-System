using Car_rental_management_system.DTOs;
using Car_rental_management_system.Enum;
using Car_rental_management_system.Models;
using Car_rental_system.Data;
using Car_rental_system.Enum;
using Car_rental_system.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
namespace Car_rental_management_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _db;

        public CarsController(IWebHostEnvironment env, ApplicationDbContext db)
        {
            _env = env;
            _db = db;
        }

        [Authorize(Roles = "CarOwner")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CarCreateDto dto)
        {
            
            var errors = new List<string>();

            if (!System.Enum.IsDefined(typeof(CarStatus), dto.Status))
                errors.Add("Status is required and must be a valid value.");

            if (string.IsNullOrWhiteSpace(dto.Model))
                errors.Add("Model is required.");
            else if (dto.Model.Length < 2 || dto.Model.Length > 50)
                errors.Add("Model must be between 2 and 50 characters.");

            if (string.IsNullOrWhiteSpace(dto.Number))
                errors.Add("Number is required.");
            

            if (string.IsNullOrWhiteSpace(dto.Color))
                errors.Add("Color is required.");
            else if (!Regex.IsMatch(dto.Color, @"^[A-Za-z]{3,20}$"))
                errors.Add("Color must contain only letters and be 3–20 characters long.");
            if (!System.Enum.IsDefined(typeof(ImageType), dto.ImageType))
                errors.Add("Invalid image type.");


            if (string.IsNullOrWhiteSpace(dto.Location))
                errors.Add("Location is required.");

            if (dto.PlanId != null)
            {
                var planExists = await _db.Plans.AnyAsync(p => p.PlanId == dto.PlanId);
                if (!planExists)
                    errors.Add("Invalid PlanId.");
            }

            if (errors.Any())
                return BadRequest(new { Errors = errors });
            var userId = User.FindFirst("UserID")?.Value;
            var admin = await _db.Users.FirstOrDefaultAsync(u => u.Role == Role.Admin);

            var adminId = admin?.Id;

            var car = new Car
            {
                Model = dto.Model,
                Number = dto.Number,
                Status = dto.Status,
                Color = dto.Color,
                Location = dto.Location,
                Type = dto.Type,
                Description=dto.Describtion,
                CarOwnerId = userId,
                AdminId = adminId,
                PlanId = dto.PlanId,
                

                CarImages = new List<Car_CarImage>(),
            };


            if (dto.Images != null && dto.Images.Any())
            {
                string folder = Path.Combine(_env.WebRootPath, "carImages");
                Directory.CreateDirectory(folder);

                foreach (var img in dto.Images)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var extension = Path.GetExtension(img.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        return BadRequest($"File type not allowed: {extension}");
                    }

                    string fileName = Guid.NewGuid() + extension;
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    car.CarImages.Add(new Car_CarImage
                    {
                        ImagePath = "/carImages/" + fileName,
                        ImageType = dto.ImageType
                    });
                }
            }

            _db.Cars.Add(car);
            await _db.SaveChangesAsync();

            return Created($"api/cars/{car.CarId}", new
            {
                car.CarId,
                car.Model,
                car.Number,
                car.Status,
                car.Color,
                car.Location,
                car.PlanId,
              
                Images = car.CarImages.Select(i => new {
                    i.ImagePath,
                    i.ImageType
                })
            });
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllCars()
        {
              
            var cars = await _db.Cars
                .Include(c => c.Plan)   
                .ToListAsync();

            var result = cars.Select(car => new CarsGetDTO
            {
                Id = car.CarId,
                Model = car.Model,
                Status = car.Status,
                Color = car.Color,
                Type = car.Type,
                PricingPlan = new PricingPlanDTO
                {
                    PricePerUnit = car.Plan.PricePerUnit,
                    Plan_type = car.Plan.Plan_type
                },

            });

            return Ok(result);
        }

        [HttpGet("Get{id:int}")]
        public async Task<IActionResult> GetCarByID( int id )
        {

            var cars = await _db.Cars
                .Include(c => c.Plan)
                .ToListAsync();

            var result = cars.Select(car => new CarsGetDTOById
            {
                Id = car.CarId,
                Model = car.Model,
                Status = car.Status,
                Color = car.Color,
                Type = car.Type,
                Describtion = car.Description,
                PricingPlan = new PricingPlanDTO
                {
                    PricePerUnit = car.Plan.PricePerUnit,
                    Plan_type = car.Plan.Plan_type
                }
            });

           var Result= result.FirstOrDefault(result => result.Id==id);
            return Ok(Result);
        }

        [HttpGet("GetImages/{id:int}")]
        public async Task<IActionResult> GetCarImages(int id)
        {
            var car = await _db.Cars
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync(c => c.CarId == id);

            if (car == null)
                return NotFound("Car not found");

            var result = new CarImageDTO
            {
                Id = car.CarId,
                ImagePath = car.CarImages.Select(i => i.ImagePath).ToList()
            };

            return Ok(result);
        }
        [HttpGet("GetOneImage/{id:int}")]
        public async Task<IActionResult> GetCarImage(int id)
        {
            var car = await _db.Cars
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync(c => c.CarId == id);

            if (car == null)
                return NotFound("Car not found");

            var result = new CarOneImageDTO
            {
                Id = car.CarId,
                ImagePath = car.CarImages.Select(i => i.ImagePath).FirstOrDefault()
            };

            return Ok(result);
        }
        [HttpGet("FilterByAvailability")]
        public async Task<IActionResult> FilterByAvailability( string status)
        {

            var cars = await _db.Cars
                .Include(c => c.Plan)
                .ToListAsync();

            var result = cars.Select(car => new CarsGetDTO
            {
                Id = car.CarId,
                Model = car.Model,
                Status = car.Status,
                Color = car.Color,
                Type = car.Type,
                PricingPlan = new PricingPlanDTO
                {
                    PricePerUnit = car.Plan.PricePerUnit,
                    Plan_type = car.Plan.Plan_type
                },

            });
            var parsedStatus = System.Enum.Parse<CarStatus>(status, true);

            var Result = result.Where(result => result.Status == parsedStatus);
            return Ok(Result);
            
        }
        [HttpGet("FilterByColor")]
        public async Task<IActionResult> GetCarByColor(string color)
        {

            var cars = await _db.Cars
                .Include(c => c.Plan)
                .ToListAsync();

            var result = cars.Select(car => new CarsGetDTO
            {
                Id = car.CarId,
                Model = car.Model,
                Status = car.Status,
                Color = car.Color,
                Type = car.Type,
                PricingPlan = new PricingPlanDTO
                {
                    PricePerUnit = car.Plan.PricePerUnit,
                    Plan_type = car.Plan.Plan_type
                },

            });
        

            var Result = result.Where(result => result.Color.ToLower() == color);
            return Ok(Result);

        }
        [HttpGet("FilterByType")]
        public async Task<IActionResult> GetCarByType(string type)
        {

            var cars = await _db.Cars
                .Include(c => c.Plan)
                .ToListAsync();

            var result = cars.Select(car => new CarsGetDTO
            {
                Id = car.CarId,
                Model = car.Model,
                Status = car.Status,
                Color = car.Color,
                Type = car.Type,
                PricingPlan = new PricingPlanDTO
                {
                    PricePerUnit = car.Plan.PricePerUnit,
                    Plan_type = car.Plan.Plan_type
                },

            });


            var Result = result.Where(result => result.Type.ToLower() == type);
            return Ok(Result);

        }
        [HttpGet("FilterByPrice")]
        public async Task<IActionResult> GetCarByPrice(double price)
        {

            var cars = await _db.Cars
                .Include(c => c.Plan)
                .ToListAsync();

            var result = cars.Select(car => new CarsGetDTO
            {
                Id = car.CarId,
                Model = car.Model,
                Status = car.Status,
                Color = car.Color,
                Type = car.Type,
                PricingPlan = new PricingPlanDTO
                {
                    PricePerUnit = car.Plan.PricePerUnit,
                    Plan_type = car.Plan.Plan_type
                },

            });

            var Result = result.Where(result => result.PricingPlan.PricePerUnit == price);
            return Ok(Result);

        }
        [HttpGet("FilterByPlan_type")]
        public async Task<IActionResult> GetCarByPlan_type(string plan_type)
        {

            var cars = await _db.Cars
                .Include(c => c.Plan)
                .ToListAsync();

            var result = cars.Select(car => new CarsGetDTO
            {
                Id = car.CarId,
                Model = car.Model,
                Status = car.Status,
                Color = car.Color,
                Type = car.Type,
                PricingPlan = new PricingPlanDTO
                {
                    PricePerUnit = car.Plan.PricePerUnit,
                    Plan_type = car.Plan.Plan_type
                },

            });
            var parsedPlan_type = System.Enum.Parse<plan_type>(plan_type, true);

            var Result = result.Where(result => result.PricingPlan.Plan_type == parsedPlan_type);

            return Ok(Result);

        }
        //[HttpGet("CheckAvailability{id:int}")]
        //public async Task<IActionResult> CheckAvailability(int id)
        //{

        //    var cars = await _db.Cars
        //        .Include(c => c.Plan)
        //        .ToListAsync();

        //    var result = cars.Select(car => new CheckCarAvaialbilityDTO
        //    {
        //        Status = car.Status,
               
        //    });

        //    var Result = result.FirstOrDefault(result => result.Id == id);
        //    return Ok(Result);
        //}





    }
}
