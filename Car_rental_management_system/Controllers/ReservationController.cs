using Car_rental_management_system.DTOs;
using Car_rental_management_system.Enum;
using Car_rental_management_system.Models;
using Car_rental_system.Data;
using Car_rental_system.Enum;
using Car_rental_system.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Car_rental_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ReservationController(ApplicationDbContext context)
        {
            _db = context;

        }
        [Authorize(Roles = "Customer")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateReservation(ReservationDto request)
        {
            var car = await _db.Cars
                .Include(c => c.Plan)
                .FirstOrDefaultAsync(c => c.CarId == request.CarId);

            if (car == null)
                return NotFound("Car not found");

            if (car.Plan == null)
                return BadRequest("Car has no pricing plan assigned.");


            if (car.Status != CarStatus.Available)
                return BadRequest("Car not available");

            if (car.Status != CarStatus.Available)
                return BadRequest("Car not available");

            var start = request.PickupStartDate.ToDateTime(request.pickupStartTime);
            var end = request.PickupEndDate.ToDateTime(request.pickupEndTime);

            if (start >= end)
                return BadRequest("End time must be after start time");
            bool hasOverlap = await _db.Reservations.AnyAsync(r =>
                r.CarId == request.CarId &&

               (
                   r.PickupStartDate < request.PickupEndDate ||
                   (r.PickupStartDate == request.PickupEndDate &&
                    r.PickupStartTime < request.pickupEndTime)
               )

               &&

               (
                   r.PickupEndDate > request.PickupStartDate ||
                   (r.PickupEndDate == request.PickupStartDate &&
                    r.PickupEndTime > request.pickupStartTime)
               )
            );

            if (hasOverlap)
                return BadRequest("Car already reserved in this period");

            var userId = User.FindFirst("UserID")?.Value;
            var admin = await _db.Users.FirstOrDefaultAsync(u => u.Role == Role.Admin);

            var adminId = admin?.Id;

            var reservation = new Reservation
            {
                CarId = request.CarId,
                Status = ReservationStatus.Approved,
                Car = car,
                CustomerId = userId,
                PickupStartDate = request.PickupStartDate,
                PickupStartTime = request.pickupStartTime,
                PickupEndDate = request.PickupEndDate,
                PickupEndTime = request.pickupEndTime,
                AdminId = adminId,
                PickupLocation = request.PickupLocation,
                DropoffLocation = request.DropoffLocation
            };


            reservation.Final_amount = reservation.CalculateFinalAmount();

            _db.Reservations.Add(reservation);

            car.Status = CarStatus.Unavailable;

            await _db.SaveChangesAsync();

            return Ok(new ReservationDto
            {
                CarId = reservation.CarId,
                PickupStartDate = reservation.PickupStartDate,
                PickupEndDate = reservation.PickupEndDate,
                pickupStartTime = reservation.PickupStartTime,
                pickupEndTime = reservation.PickupEndTime,
                PickupLocation = reservation.PickupLocation,
                DropoffLocation = reservation.DropoffLocation


            });
        }
        [HttpGet("GetReservation/{id:int}")]
        public async Task<IActionResult> GetReservationByID(int id)
        {
            var reservation = await _db.Reservations
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)  
                .Include(r => r.Car)
                    .ThenInclude(c => c.Plan)  
                .Where(r => r.Id == id)
                .Select(r => new ReservationGetDto
                {
                    ReservationId = r.Id,
                    CustomerName = r.Customer.User.UserName,
                    CustomerPhone = r.Customer.User.PhoneNumber,
                    CustomerLicenseNumber = r.Customer.User.LicenseNumber,
                    CustomerCity = r.Customer.User.City,

                    CarId = r.Car.CarId,
                    CarModel = r.Car.Model,
                    ReservationStatus = r.Status,
                    CarPrice = r.Final_amount,
                    CarLocationPickup = r.PickupLocation,
                    CarLocationDropoff = r.DropoffLocation,

                    PickupDateTime = r.PickupStartDateTime,
                    DropoffDateTime = r.PickupEndDateTime
                })
                .FirstOrDefaultAsync();

            if (reservation == null)
                return NotFound($"Reservation with Id {id} not found.");

            return Ok(reservation);
        }

    }
}
