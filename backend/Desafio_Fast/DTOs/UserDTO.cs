using System.ComponentModel.DataAnnotations;

namespace Desafio_Fast.DTOs
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "Username é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username deve ter entre 3 e 100 caracteres")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        [StringLength(255, ErrorMessage = "Email deve ter no máximo 255 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password deve ter entre 6 e 100 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmação de password é obrigatória")]
        [Compare("Password", ErrorMessage = "Password e confirmação devem ser iguais")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class LoginUserDTO
    {
        [Required(ErrorMessage = "Username ou Email é obrigatório")]
        public string UsernameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password é obrigatória")]
        public string Password { get; set; } = string.Empty;
    }
}