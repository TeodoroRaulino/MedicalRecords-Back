using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly APIDbContext _context;

        public AuthController(APIDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Authenticate([FromBody] LoginDTO loginDTO)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
            {
                return Unauthorized("Invalid email or password");
            }

            var token = TokenService.GenerateToken(user);

            user.Password = "";

            return Ok(new { token, user });
        }
    }
}
