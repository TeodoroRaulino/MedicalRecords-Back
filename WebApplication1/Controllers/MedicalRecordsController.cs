using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Migrations;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/medical-records")]
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
        public async Task<ActionResult<IEnumerable<GetMedicalRecordsDTO>>> GetMedicalRecords()
        {
            try
            {
                var medicalRecords = await _context.MedicalRecords.ToListAsync();

                var medicalRecordsDTOs = medicalRecords.Select(item =>
                {
                    var address = item.Address;
                    var addressDTO = address != null ? new AddressDTO
                    {
                        Street = address.Street,
                        Neighborhood = address.Neighborhood,
                        City = address.City,
                        State = address.State,
                        PostalCode = address.PostalCode
                    } : null;

                    return new GetMedicalRecordsDTO
                    {
                        Id = item.Id,
                        PhotoPath = item.PhotoPath,
                        FullName = item.FullName,
                        CPF = item.CPF,
                        PhoneNumber = item.PhoneNumber,
                        Address = addressDTO,
                        UserId = item.UserId
                    };
                }).ToList();

                foreach (var item in medicalRecordsDTOs)
                {
                    var fileInfo = new FileInfo(Path.Combine("D:\\Vscode\\medical\\WebApplication1\\WebApplication1\\Images\\", item.PhotoPath));
                    var data = new byte[fileInfo.Length];
                    using (FileStream fs = fileInfo.OpenRead())
                    {
                        fs.Read(data, 0, data.Length);
                    }

                    item.Photo = data;
                }

                return medicalRecordsDTOs;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao recuperar os registros médicos.");
            }
        }


        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<MedicalRecords>> PostMedicalRecords([FromForm] MedicalRecordsDTO medicalRecordsDTO)
        {
            try
            {
                if (_context.MedicalRecords == null)
                {
                    return Problem("Entity set 'APIDbContext.MedicalRecords' is null.");
                }

                string path = Path.Combine("D:\\Vscode\\medical\\WebApplication1\\WebApplication1\\Images\\", medicalRecordsDTO.PhotoPath);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await medicalRecordsDTO.Photo.CopyToAsync(stream);
                }

                var user = await _context.User.FirstOrDefaultAsync(u => u.Id == medicalRecordsDTO.UserId);
                if (user == null)
                {
                    return BadRequest("Usuário não encontrado");
                }
                var address = new Address();
                if (medicalRecordsDTO.Address != null)
                {
                    address.Street = medicalRecordsDTO.Address.Street;
                    address.Neighborhood = medicalRecordsDTO.Address.Neighborhood;
                    address.City = medicalRecordsDTO.Address.City;
                    address.State = medicalRecordsDTO.Address.State;
                    address.PostalCode = medicalRecordsDTO.Address.PostalCode;
                }

                var medicalRecords = new MedicalRecords
                {
                    PhotoPath = medicalRecordsDTO.PhotoPath,
                    FullName = medicalRecordsDTO.FullName,
                    CPF = medicalRecordsDTO.CPF,
                    PhoneNumber = medicalRecordsDTO.PhoneNumber,
                    Address = address,
                    UserId = user.Id
                };

                _context.MedicalRecords.Add(medicalRecords);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMedicalRecords", new { id = medicalRecords.Id }, medicalRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao salvar o registro médico.");
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<MedicalRecords>> GetMedicalRecords(int id)
        {
            try
            {
                var medicalRecords = await _context.MedicalRecords.FirstOrDefaultAsync(m => m.Id == id);

                if (medicalRecords == null)
                {
                    return NotFound();
                }

                return Ok(medicalRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao recuperar o registro médico.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateMedicalRecords(int id, [FromForm] int userId)
        {
            try
            {
                var existingMedicalRecords = await _context.MedicalRecords.FindAsync(id);
                if (existingMedicalRecords == null)
                {
                    return NotFound();
                }

                var existingUser = await _context.User.FindAsync(userId);
                if (existingUser == null)
                {
                    return BadRequest("Usuário não encontrado");
                }

                existingMedicalRecords.UserId = existingUser.Id;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao atualizar o registro médico.");
            }
        }




        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DeleteMedicalRecords(int id)
        {
            try
            {
                var medicalRecords = await _context.MedicalRecords.FindAsync(id);
                if (medicalRecords == null)
                {
                    return NotFound();
                }

                _context.MedicalRecords.Remove(medicalRecords);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao deletar o registro médico.");
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<MedicalRecords>> GetMedicalRecordsByUserId(int userId)
        {
            try
            {
                var medicalRecords = await _context.MedicalRecords.FirstOrDefaultAsync(m => m.UserId == userId);

                if (medicalRecords == null)
                {
                    return NotFound();
                }

                return Ok(medicalRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao recuperar os dados do registro médico.");
            }
        }

        [HttpGet("my-medical-records")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<GetMedicalRecordsDTO>> GetMedicalRecordsByUserId()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var medicalRecord = await _context.MedicalRecords.FirstOrDefaultAsync(m => m.UserId == userId);

                if (medicalRecord == null)
                {
                    return NotFound();
                }

                var address = medicalRecord.Address;
                var addressDTO = address != null ? new AddressDTO
                {
                    Street = address.Street,
                    Neighborhood = address.Neighborhood,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode
                } : null;

                var imagePath = Path.Combine("D:\\Vscode\\medical\\WebApplication1\\WebApplication1\\Images\\", medicalRecord.PhotoPath);
                var fileInfo = new FileInfo(imagePath);
                var data = new byte[fileInfo.Length];

                using (FileStream fs = fileInfo.OpenRead())
                {
                    fs.Read(data, 0, data.Length);
                }

                var medicalRecordDTO = new GetMedicalRecordsDTO
                {
                    Id = medicalRecord.Id,
                    PhotoPath = medicalRecord.PhotoPath,
                    FullName = medicalRecord.FullName,
                    CPF = medicalRecord.CPF,
                    PhoneNumber = medicalRecord.PhoneNumber,
                    Address = addressDTO,
                    UserId = medicalRecord.UserId,
                    Photo = data
                };

                return Ok(medicalRecordDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao recuperar os dados do registro médico. Por favor, tente novamente mais tarde.");
            }
        }



    }
}
