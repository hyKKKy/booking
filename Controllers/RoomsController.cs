using booking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using booking.Entities;
using Microsoft.EntityFrameworkCore;
using booking.DTOs.Rooms;

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

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoom(Guid id, UpdateRoomDto dto)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            var roomType = await _context.RoomTypes.FindAsync(dto.RoomTypeId);
            if (roomType == null)
            {
                return NotFound($"RoomType with ID {dto.RoomTypeId} not found.");
            }

            if (roomType.HotelId != room.RoomType.HotelId)
            {
                return BadRequest("Room type belongs to a different hotel.");
            }
            bool roomExists = await _context.Rooms.AnyAsync(r => r.Id != id && r.Number == dto.Number && r.RoomType.HotelId == roomType.HotelId);
            if (roomExists)
            {
                return Conflict($"A room with the number '{dto.Number}' already exists in this hotel.");
            }
            // TODO: before changing RoomTypeId, check bookings via booking service
            room.Number = dto.Number;
            room.RoomTypeId = dto.RoomTypeId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRooms([FromQuery] Guid? hotelId, [FromQuery] Guid? roomTypeId)
        {
            IQueryable<Room> query = _context.Rooms;

            if (hotelId.HasValue)
            {
                query = query.Where(r => r.RoomType.HotelId == hotelId.Value);
            }

            if (roomTypeId.HasValue)
            {
                query = query.Where(r => r.RoomTypeId == roomTypeId.Value);
            }

            var rooms = await query
                .OrderBy(r => r.Number)
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    Number = r.Number,
                    RoomTypeId = r.RoomTypeId
                })
                .ToListAsync();

            return Ok(rooms);
        }
    }
}
