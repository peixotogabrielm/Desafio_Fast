using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ControleAtas.Models;

public class Colaborador
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Ata> Atas { get; set; } = new List<Ata>();
}
