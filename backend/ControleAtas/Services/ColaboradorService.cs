using ControleAtas.DTOs;
using ControleAtas.Models;
using ControleAtas.Repositories.Interfaces;
using ControleAtas.Services.Interfaces;

namespace ControleAtas.Services;

public class ColaboradorService : IColaboradorService
{
    private readonly IColaboradorRepository _colaboradorRepository;

    public ColaboradorService(IColaboradorRepository colaboradorRepository)
    {
        _colaboradorRepository = colaboradorRepository;
    }

    public async Task<ColaboradorSummaryDto> CreateColaboradorAsync(ColaboradorCreateDto request)
    {
        var colaborador = new Colaborador
        {
            Nome = request.Nome
        };

        await _colaboradorRepository.AddAsync(colaborador);

        return new ColaboradorSummaryDto
        {
            Id = colaborador.Id,
            Nome = colaborador.Nome
        };
    }

    public async Task<IEnumerable<ColaboradorResponseDto>> GetColaboradoresAsync()
    {
        var colaboradores = await _colaboradorRepository.GetAllWithAtasAsync();

        return colaboradores.Select(c => new ColaboradorResponseDto
        {
            Id = c.Id,
            Nome = c.Nome,
            Workshops = c.Atas
                .Where(a => a.Workshop is not null)
                .GroupBy(a => a.WorkshopId)
                .Select(g =>
                {
                    var workshop = g.First().Workshop!;
                    return new WorkshopSummaryDto
                    {
                        Id = workshop.Id,
                        Nome = workshop.Nome,
                        DataRealizacao = workshop.DataRealizacao
                    };
                })
                .OrderBy(w => w.Nome)
                .ToList()
        });
    }
}
