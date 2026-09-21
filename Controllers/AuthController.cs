using booking.Services;
using Microsoft.AspNetCore.Mvc;
using booking.Data;
using booking.DTOs;
using booking.Entities;
using Microsoft.EntityFrameworkCore;

namespace booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        public AuthController(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            bool emailTaken = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if(emailTaken)
            {
                return BadRequest("Email is already taken.");
            }
            
            string hashedPassword = _passwordHasher.Hash(dto.Password);

            User user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = hashedPassword
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
