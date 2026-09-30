using booking.Common;

namespace booking.Services.Pricing
{
    public interface IPricingService
    {
        Task<Result<decimal>> CalculateTotalAsync(Guid roomTypeId, DateOnly checkIn, DateOnly checkOut);
    }
}
