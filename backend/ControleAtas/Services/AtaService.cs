using ControleAtas.DTOs;
using ControleAtas.Models;
using ControleAtas.Repositories.Interfaces;
using ControleAtas.Services.Exceptions;
using ControleAtas.Services.Interfaces;

namespace ControleAtas.Services;

public class AtaService : IAtaService
{
    private readonly IAtaRepository _ataRepository;
    private readonly IWorkshopRepository _workshopRepository;
    private readonly IColaboradorRepository _colaboradorRepository;

    public AtaService(
        IAtaRepository ataRepository,
        IWorkshopRepository workshopRepository,
        IColaboradorRepository colaboradorRepository)
    {
        _ataRepository = ataRepository;
        _workshopRepository = workshopRepository;
        _colaboradorRepository = colaboradorRepository;
    }

    public async Task<AtaResponseDto> CreateAtaAsync(AtaCreateDto request)
    {
        var workshop = await _workshopRepository.GetByIdAsync(request.WorkshopId);
        if (workshop is null)
        {
            throw new EntityNotFoundException($"Workshop {request.WorkshopId} não encontrado.");
        }

        var ata = new Ata
        {
            WorkshopId = request.WorkshopId
        };

        if (request.ColaboradorIds.Count > 0)
        {
            var colaboradores = await _colaboradorRepository.GetByIdsAsync(request.ColaboradorIds);
            var missing = request.ColaboradorIds.Except(colaboradores.Select(c => c.Id)).ToList();
            if (missing.Count > 0)
            {
                throw new EntityNotFoundException("Alguns colaboradores não foram encontrados.", missing);
            }

            foreach (var colaborador in colaboradores)
            {
                ata.Colaboradores.Add(colaborador);
            }
        }

        await _ataRepository.AddAsync(ata);

        var created = await _ataRepository.GetByIdWithColaboradoresAsync(ata.Id);
        return MapToResponse(created ?? ata, workshop);
    }

    public async Task AddColaboradorAsync(int ataId, int colaboradorId)
    {
        var ata = await _ataRepository.GetByIdWithColaboradoresAsync(ataId);
        if (ata is null)
        {
            throw new EntityNotFoundException($"Ata {ataId} não encontrada.");
        }

        var colaborador = await _colaboradorRepository.GetByIdAsync(colaboradorId   );
        if (colaborador is null)
        {
            throw new EntityNotFoundException($"Colaborador {colaboradorId} não encontrado.");
        }

        if (ata.Colaboradores.All(c => c.Id != colaboradorId))
        {
            ata.Colaboradores.Add(colaborador);
            await _ataRepository.SaveChangesAsync();
        }
    }

    public async Task RemoveColaboradorAsync(int ataId, int colaboradorId)
    {
        var ata = await _ataRepository.GetByIdWithColaboradoresAsync(ataId);
        if (ata is null)
        {
            throw new EntityNotFoundException($"Ata {ataId} não encontrada.");
        }

        var colaborador = ata.Colaboradores.FirstOrDefault(c => c.Id == colaboradorId);
        if (colaborador is null)
        {
            throw new EntityNotFoundException($"Colaborador {colaboradorId} não está associado à ata {ataId}.");
        }

        ata.Colaboradores.Remove(colaborador);
        await _ataRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<AtaResponseDto>> GetAtasAsync(string? workshopNome, DateTime? data)
    {
        var atas = await _ataRepository.GetAtasAsync(workshopNome, data);
        return atas.Select(MapToResponse);
    }

    private static AtaResponseDto MapToResponse(Ata ata)
    {
        var workshop = ata.Workshop;
        return new AtaResponseDto
        {
            Id = ata.Id,
            Workshop = workshop is null
                ? null
                : new WorkshopSummaryDto
                {
                    Id = workshop.Id,
                    Nome = workshop.Nome,
                    DataRealizacao = workshop.DataRealizacao
                },
            Colaboradores = ata.Colaboradores
                .OrderBy(c => c.Nome)
                .Select(c => new ColaboradorSummaryDto
                {
                    Id = c.Id,
                    Nome = c.Nome
                })
                .ToList()
        };
    }

    private static AtaResponseDto MapToResponse(Ata ata, Workshop workshop)
    {
        ata.Workshop ??= workshop;
        return MapToResponse(ata);
    }
}
