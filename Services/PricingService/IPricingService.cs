using booking.Common;

namespace booking.Services.PricingService.PricingService
{
    public interface IPricingService
    {
        Task<Result<decimal>> CalculateTotalAsync(Guid roomTypeId, DateOnly checkIn, DateOnly checkOut);
    }
}
