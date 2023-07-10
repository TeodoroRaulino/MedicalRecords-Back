using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class MedicalRecords
    {
        public int Id { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
        public string PhotoPath { get; set; }

        public string FullName { get; set; }

        public string CPF { get; set; }

        public string PhoneNumber { get; set; }

        public string? Street { get; set; }

        public string? Neighborhood { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PostalCode { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
