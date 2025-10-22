using System.ComponentModel.DataAnnotations;

namespace ControleAtas.DTOs;

public class ColaboradorCreateDto
{
    public string Nome { get; set; } = string.Empty;
}

public class ColaboradorResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public List<WorkshopSummaryDto> Workshops { get; set; } = new();
}

public class WorkshopCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public DateTime DataRealizacao { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class WorkshopResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataRealizacao { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

public class AtaCreateDto
{
    public int WorkshopId { get; set; }
    public List<int> ColaboradorIds { get; set; } = new();
}

public class AtaResponseDto
{
    public int Id { get; set; }
    public WorkshopSummaryDto? Workshop { get; set; }
    public List<ColaboradorSummaryDto> Colaboradores { get; set; } = new();
}

public class WorkshopSummaryDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataRealizacao { get; set; }
}

public class ColaboradorSummaryDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}

public class AuthRequestDto
{
    [Required(ErrorMessage = "Campo email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Campo senha é obrigatório")]
    [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
    public string Password { get; set; } = string.Empty;
}

public class RegistrarRequestDto
{
    [Required(ErrorMessage = "Campo nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;
    [Required(ErrorMessage = "Campo email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Campo senha é obrigatório")]
    [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
