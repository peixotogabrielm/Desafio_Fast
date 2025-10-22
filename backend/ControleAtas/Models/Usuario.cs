using System.ComponentModel.DataAnnotations;

namespace ControleAtas.Models
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string Senha { get; set; }
    }
}
