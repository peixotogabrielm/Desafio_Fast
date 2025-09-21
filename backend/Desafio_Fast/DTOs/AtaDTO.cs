using System.ComponentModel.DataAnnotations;

namespace Desafio_Fast.DTOs
{
    public class AtaDTO
    {
        public int Id { get; set; }
        public WorkshopDTO Workshop { get; set; } = null!;
        public List<ColaboradorDTO> Colaboradores { get; set; } = new List<ColaboradorDTO>();
    }

    public class CreateAtaDTO
    {
        [Required(ErrorMessage = "Workshop ID é obrigatório")]
        public int WorkshopId { get; set; }
    }

    public class AtaComColaboradoresDTO
    {
        public int Id { get; set; }
        public string WorkshopNome { get; set; } = string.Empty;
        public DateTime DataRealizacao { get; set; }
        public List<ColaboradorDTO> Colaboradores { get; set; } = new List<ColaboradorDTO>();
    }
}