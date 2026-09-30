using booking.Common;

namespace booking.Services
{
    public interface IPricingService
    {
        Task<Result<decimal>> CalculateTotalAsync(Guid roomTypeId, DateOnly checkIn, DateOnly checkOut);
    }
}
