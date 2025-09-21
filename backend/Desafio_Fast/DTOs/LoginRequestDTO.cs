using System.ComponentModel.DataAnnotations;

namespace Desafio_Fast.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username é obrigatório")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password é obrigatória")]
        public string Password { get; set; } = string.Empty;
    }
}
