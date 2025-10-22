using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ControleAtas.Models;

public class Workshop
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;
    [DataType(DataType.Date)]
    public DateTime DataRealizacao { get; set; }
    [MaxLength(500)]
    public string Descricao { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Ata> Atas { get; set; } = new List<Ata>();
}
