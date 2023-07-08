using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly APIDbContext _context;

        public UsersController(APIDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            try
            {
                if (_context.User == null)
                {
                    return NotFound();
                }

                return await _context.User.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao recuperar os usuários.");
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                if (_context.User == null)
                {
                    return NotFound();
                }

                var user = await _context.User.FindAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao recuperar o usuário.");
            }
        }


        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> PutUser(int id,[FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Usuário editado com sucesso.");
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {
          if (_context.User == null)
          {
              return Problem("Entity set 'APIDbContext.User'  is null.");
          }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            var response = new
            {
                id = user.Id,
                message = "Usuário criado com sucesso."
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("dashboard")]
        [Authorize(Roles = "Doctor")]
        public ActionResult<DashboardInfoDTO> GetDashboardInfo()
        {
            try
            {
                var totalDoctors = _context.User.Count(u => u.Role == Enums.UserRole.Doctor);
                var totalPatients = _context.User.Count(u => u.Role == Enums.UserRole.Patient);
                var totalMedicalRecords = _context.MedicalRecords.Count();

                var dashboardInfo = new DashboardInfoDTO
                {
                    TotalDoctors = totalDoctors,
                    TotalPatients = totalPatients,
                    TotalMedicalRecords = totalMedicalRecords
                };

                return Ok(dashboardInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao recuperar as informações do dashboard.");
            }
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
