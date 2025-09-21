using System.ComponentModel.DataAnnotations;

namespace Desafio_Fast.Models
{
    public class Workshop
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;
        
        [Required]
        public DateTime DataRealizacao { get; set; }
        
        [StringLength(1000)]
        public string Descricao { get; set; } = string.Empty;
        
        public virtual ICollection<Ata> Atas { get; set; } = new List<Ata>();
    }
}