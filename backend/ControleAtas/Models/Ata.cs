using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ControleAtas.Models;

public class Ata
{
    [Key]
    public int Id { get; set; }
    [MaxLength(200)]
    public int WorkshopId { get; set; }
    
    public Workshop? Workshop { get; set; }

    public ICollection<Colaborador> Colaboradores { get; set; } = new List<Colaborador>();
}
