using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Migrations;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly APIDbContext _context;

        public MedicalRecordsController(APIDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<IEnumerable<MedicalRecords>>> GetMedicalRecords()
        {
            if (_context.MedicalRecords == null)
            {
                return NotFound();
            }
            return await _context.MedicalRecords.ToListAsync();

        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<MedicalRecords>> PostMedicalRecords([FromForm] MedicalRecords medicalRecords)
        {
            if (_context.MedicalRecords == null)
            {
                return Problem("Entity set 'APIDbContext.MedicalRecords'  is null.");
            }

            string path = Path.Combine("D:\\Images to DataBase", medicalRecords.PhotoPath);
            using(Stream stream = new FileStream(path, FileMode.Create))
            {
                medicalRecords.Photo.CopyTo(stream);
            }   
            
            
            _context.MedicalRecords.Add(medicalRecords);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicalRecords", new { id = medicalRecords.Id }, medicalRecords);
        }

    }
}
