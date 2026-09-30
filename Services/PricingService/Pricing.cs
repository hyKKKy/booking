using booking.Common;
using booking.Data;
using Microsoft.EntityFrameworkCore;

namespace booking.Services.PricingService
{
    public class PricingService : IPricingService
    {
        private readonly AppDbContext _context;

        public PricingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<decimal>> CalculateTotalAsync(Guid roomTypeId, DateOnly checkIn, DateOnly checkOut)
        {
            if (checkOut <= checkIn)
            {
                return Result<decimal>.Failure(ErrorType.Validation, "Check-out must be after check-in.");
            }

            decimal? basePrice = await _context.RoomTypes
                .Where(rt => rt.Id == roomTypeId)
                .Select(rt => (decimal?)rt.BasePrice)
                .FirstOrDefaultAsync();
            if (basePrice == null)
            {
                return Result<decimal>.Failure(ErrorType.NotFound, $"RoomType with ID {roomTypeId} not found.");
            }

            var overrides = await _context.PriceOverrides
                .Where(po => po.RoomTypeId == roomTypeId && po.Date >= checkIn && po.Date < checkOut)
                .ToDictionaryAsync(po => po.Date, po => po.Price);

            decimal total = 0;
            for (var night = checkIn; night < checkOut; night = night.AddDays(1))
            {
                total += overrides.TryGetValue(night, out var overridePrice) ? overridePrice : basePrice.Value;
            }

            return Result<decimal>.Success(total);
        }
    }
}