using System.ComponentModel.DataAnnotations;
using WebApplication1.Enums;

namespace WebApplication1.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O Email deve ser um endereço de email válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O nome completo do usuário é obrigatório.")]
        [StringLength(200, MinimumLength = 4, ErrorMessage = "O nome deve ter entre 4 e 200 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        public UserRole Role { get; set; }

    }
}
