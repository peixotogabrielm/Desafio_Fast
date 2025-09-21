using System.ComponentModel.DataAnnotations;

namespace Desafio_Fast.DTOs
{
    public class ColaboradorDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<WorkshopResumoDTO>? Workshops { get; set; }
    }

    public class CreateColaboradorDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
        public string Nome { get; set; } = string.Empty;
    }
}