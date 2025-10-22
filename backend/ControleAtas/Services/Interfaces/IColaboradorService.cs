using ControleAtas.DTOs;

namespace ControleAtas.Services.Interfaces;

public interface IColaboradorService
{
    Task<ColaboradorSummaryDto> CreateColaboradorAsync(ColaboradorCreateDto request);
    Task<IEnumerable<ColaboradorResponseDto>> GetColaboradoresAsync();
}
