using booking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using booking.DTOs;
using booking.Entities;
using Microsoft.EntityFrameworkCore;

namespace booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoomsController(AppDbContext context)
        {
            _context = context;
        }

        private static RoomDto ToDto(Room room)
        {
            return new RoomDto
            {
                Id = room.Id,
                Number = room.Number,
                RoomTypeId = room.RoomTypeId
            };
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoom(CreateRoomDto dto)
        {
            var roomType = await _context.RoomTypes.FindAsync(dto.RoomTypeId);
            if (roomType == null)
            {
                return NotFound($"RoomType with ID {dto.RoomTypeId} not found.");
            }

            bool roomExists = await _context.Rooms.AnyAsync(r => r.Number == dto.Number && r.RoomType.HotelId == roomType.HotelId);
            if (roomExists)
            {
                return Conflict($"A room with the number '{dto.Number}' already exists in this hotel.");
            }

            Room room = new Room
            {
                Number = dto.Number,
                RoomTypeId = dto.RoomTypeId
            };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, ToDto(room));
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoom(Guid id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(ToDto(room));
        }
    }
}
