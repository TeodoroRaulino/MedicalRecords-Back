using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class GetMedicalRecordsDTO
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoPath { get; set; }

        [Required(ErrorMessage = "O nome completo do paciente é obrigatório.")]
        [StringLength(200, MinimumLength = 4, ErrorMessage = "O nome deve ter entre 4 e 200 caracteres.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "O CPF do paciente é obrigatório.")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Informe um CPF válido no formato xxx.xxx.xxx-xx.")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O número de celular do paciente é obrigatório.")]
        [RegularExpression(@"^\+[1-9]\d{1,14}$", ErrorMessage = "Informe um número de celular válido.")]
        public string PhoneNumber { get; set; }

        public string? Street { get; set; }

        public string? Neighborhood { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        //[RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "Informe um CEP válido no formato xxxxx-xxx.")]
        public string? PostalCode { get; set; }

        public int UserId { get; set; }
    }
}
