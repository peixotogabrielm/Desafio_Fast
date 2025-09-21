using System.ComponentModel.DataAnnotations;

namespace Desafio_Fast.Models
{
    public class Ata
    {
        public int Id { get; set; }
        
        [Required]
        public int WorkshopId { get; set; }
        
        public virtual Workshop Workshop { get; set; } = null!;
        public virtual ICollection<AtaColaborador> AtaColaboradores { get; set; } = new List<AtaColaborador>();
    }
}