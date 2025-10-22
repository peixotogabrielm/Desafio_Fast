using ControleAtas.DTOs;

namespace ControleAtas.Services.Interfaces;

public interface IAtaService
{
    Task<AtaResponseDto> CreateAtaAsync(AtaCreateDto request);
    Task AddColaboradorAsync(int ataId, int colaboradorId);
    Task RemoveColaboradorAsync(int ataId, int colaboradorId);
    Task<IEnumerable<AtaResponseDto>> GetAtasAsync(string? workshopNome, DateTime? data);
}
