using booking.Entities;

namespace booking.Services.TokenService.TokenService
{
    public interface ITokenService
    {

        string GenerateToken(User user);


    }
}
