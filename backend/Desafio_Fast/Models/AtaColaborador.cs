using System.ComponentModel.DataAnnotations;

namespace Desafio_Fast.Models
{
    public class AtaColaborador
    {
        public int AtaId { get; set; }
        public int ColaboradorId { get; set; }
        
        public virtual Ata Ata { get; set; } = null!;
        public virtual Colaborador Colaborador { get; set; } = null!;
    }
}