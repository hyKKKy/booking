using booking.Common;

namespace booking.Services.AvailabilityService
{
    public interface IAvailabilityService
    {
        Task<Result<int>> GetAvailableCountAsync(Guid roomTypeId, DateOnly checkIn, DateOnly checkOut);
    }
}