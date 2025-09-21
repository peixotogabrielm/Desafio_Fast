using System.ComponentModel.DataAnnotations;

namespace Desafio_Fast.Models
{
    public class Colaborador
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;
        
        public virtual ICollection<AtaColaborador> AtaColaboradores { get; set; } = new List<AtaColaborador>();
    }
}