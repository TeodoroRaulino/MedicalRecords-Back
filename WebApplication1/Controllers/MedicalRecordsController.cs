using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly APIDbContext _DbContext;

        public MedicalRecordsController(APIDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalRecords>>> GetFichasMedicas()
        {
            var fichasMedicas = await _DbContext.MedicalRecords.ToListAsync();
            return Ok(fichasMedicas);
        }

        // Outros métodos para criar, atualizar e excluir fichas médicas

        // Exemplo de método para criar uma ficha médica
        [HttpPost]
        public async Task<ActionResult<MedicalRecords>> CreateFichaMedica(MedicalRecords fichaMedica)
        {
            _DbContext.MedicalRecords.Add(fichaMedica);
            await _DbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFichaMedicaById), new { id = fichaMedica.Id }, fichaMedica);
        }

        // Exemplo de método para obter uma ficha médica pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecords>> GetFichaMedicaById(int id)
        {
            var fichaMedica = await _DbContext.MedicalRecords.FindAsync(id);
            if (fichaMedica == null)
            {
                return NotFound();
            }
            return fichaMedica;
        }
    }
}
