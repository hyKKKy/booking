using booking.Data;
using Microsoft.AspNetCore.Mvc;
using booking.DTOs;
using booking.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HotelsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateHotel(CreateHotelDto dto)
        {
            Hotel hotel = new Hotel
            {
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                Description = dto.Description
            };

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            var hotelDto = new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                City = hotel.City,
                Description = hotel.Description
            };

            
            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotelDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetHotels()
        {
            var hotels = await _context.Hotels.ToListAsync();
            var hotelDtos = hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Address = h.Address,
                City = h.City,
                Description = h.Description
            }).ToList();
            return Ok(hotelDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotel(Guid id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            var hotelDto = new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                City = hotel.City,
                Description = hotel.Description
            };
            return Ok(hotelDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateHotel(Guid id, CreateHotelDto dto)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            hotel.Name = dto.Name;
            hotel.Address = dto.Address;
            hotel.City = dto.City;
            hotel.Description = dto.Description;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
