using booking.Entities;

namespace booking.Services
{
    public interface ITokenService
    {

        string GenerateToken(User user);


    }
}
