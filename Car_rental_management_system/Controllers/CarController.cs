using Car_rental_management_system.DTOs;
using Car_rental_management_system.Enum;
using Car_rental_management_system.Models;
using Car_rental_system.Data;
using Car_rental_system.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CarCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var car = new Car
            {
                Model = dto.Model,
                Number = dto.Number,
                Status = dto.Status,
                Color = dto.Color,
                Location = dto.Location,
                Type = dto.Type,
                OwnerId = dto.OwnerId,
                AdminId = dto.AdminId,
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
                car.OwnerId,
                car.AdminId,
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
