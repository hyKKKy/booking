using booking.Entities;

namespace booking.Services.Auth
{
    public interface ITokenService
    {

        string GenerateToken(User user);


    }
}
