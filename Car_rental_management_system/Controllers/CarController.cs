using Car_rental_management_system.DTOs;
using Car_rental_management_system.Models;
using Car_rental_system.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
