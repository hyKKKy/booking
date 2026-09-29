using booking.Data;
using Microsoft.AspNetCore.Mvc;
using booking.DTOs;
using booking.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomTypesController : ControllerBase
    {

        private const int SqlUniqueIndexViolation = 2601;
        private const int SqlUniqueConstraintViolation = 2627;

        private readonly AppDbContext _context;
        
        public RoomTypesController(AppDbContext context)
        {
            _context = context;
        }

        private static RoomTypeDto ToDto(RoomType roomType)
        {
            return new RoomTypeDto
            {
                Id = roomType.Id,
                Name = roomType.Name,
                Capacity = roomType.Capacity,
                BasePrice = roomType.BasePrice,
                Description = roomType.Description,
                HotelId = roomType.HotelId
            };
        }

        private static bool IsUniqueViolation(DbUpdateException ex)
        {
            return ex.InnerException is SqlException sql && (sql.Number == SqlUniqueIndexViolation || sql.Number == SqlUniqueConstraintViolation);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetRoomType(Guid id)
        {
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }
            var roomTypeDto = ToDto(roomType);
            return Ok(roomTypeDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoomType(CreateRoomTypeDto dto)
        {
            bool hotelExists = await _context.Hotels.AnyAsync(h => h.Id == dto.HotelId);
            if (!hotelExists)
            {
                return NotFound($"Hotel with ID {dto.HotelId} not found.");
            }

            bool isDuplicateName = await _context.RoomTypes
                .AnyAsync(rt => rt.HotelId == dto.HotelId && rt.Name == dto.Name);
            if (isDuplicateName)
            {
                return Conflict($"A room type with the name '{dto.Name}' already exists for this hotel.");
            }
            RoomType roomType = new RoomType
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                BasePrice = dto.BasePrice,
                Description = dto.Description,
                HotelId = dto.HotelId
            };
            _context.RoomTypes.Add(roomType);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                return Conflict($"A room type with the name '{dto.Name}' already exists for this hotel.");
            }
            var roomTypeDto = ToDto(roomType);
            return CreatedAtAction(nameof(GetRoomType), new { id = roomType.Id }, roomTypeDto);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoomType(Guid id, UpdateRoomTypeDto dto)
        {
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }
            bool isDuplicateName = await _context.RoomTypes
                .AnyAsync(rt => rt.HotelId == roomType.HotelId && rt.Name == dto.Name && rt.Id != id);
            if (isDuplicateName)
            {
                return Conflict($"A room type with the name '{dto.Name}' already exists for this hotel.");
            }
            roomType.Name = dto.Name;
            roomType.Capacity = dto.Capacity;
            roomType.BasePrice = dto.BasePrice;
            roomType.Description = dto.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                return Conflict($"A room type with the name '{dto.Name}' already exists for this hotel.");
            }
            return NoContent();
        }
    }
}
