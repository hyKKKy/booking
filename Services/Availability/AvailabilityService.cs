using booking.Common;
using booking.Data;
using booking.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace booking.Services.Availability
{
    public class AvailabilityService : IAvailabilityService
    {
        private static readonly Status[] ActiveStatuses =
        {
            Status.Pending,
            Status.Confirmed,
            Status.CheckedIn
        };

        private readonly AppDbContext _context;

        public AvailabilityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> GetAvailableCountAsync(Guid roomTypeId, DateOnly checkIn, DateOnly checkOut)
        {
            if (checkOut <= checkIn)
            {
                return Result<int>.Failure(ErrorType.Validation, "Check-out must be after check-in.");
            }

            int? roomCount = await _context.RoomTypes
                .Where(rt => rt.Id == roomTypeId)
                .Select(rt => (int?)rt.Rooms.Count)
                .FirstOrDefaultAsync();
            if (roomCount == null)
            {
                return Result<int>.Failure(ErrorType.NotFound, $"RoomType with ID {roomTypeId} not found.");
            }

            var bookings = await _context.Bookings
                .Where(b => b.RoomTypeId == roomTypeId
                    && ActiveStatuses.Contains(b.Status)
                    && b.CheckIn < checkOut && b.CheckOut > checkIn)
                .Select(b => new { b.CheckIn, b.CheckOut })
                .ToListAsync();

            int maxOccupied = 0;
            for (var night = checkIn; night < checkOut; night = night.AddDays(1))
            {
                int occupied = bookings.Count(b => b.CheckIn <= night && b.CheckOut > night);
                if (occupied > maxOccupied)
                {
                    maxOccupied = occupied;
                }
            }

            int available = roomCount.Value - maxOccupied;
            return Result<int>.Success(Math.Max(available, 0));
        }
    }
}