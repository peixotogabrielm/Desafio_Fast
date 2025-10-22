using ControleAtas.DTOs;
using ControleAtas.Models;
using ControleAtas.Repositories.Interfaces;
using ControleAtas.Services.Exceptions;
using ControleAtas.Services.Interfaces;

namespace ControleAtas.Services;

public class WorkshopService : IWorkshopService
{
    private readonly IWorkshopRepository _workshopRepository;

    public WorkshopService(IWorkshopRepository workshopRepository)
    {
        _workshopRepository = workshopRepository;
    }

    public async Task<WorkshopResponseDto> CreateWorkshopAsync(WorkshopCreateDto request)
    {
        var workshop = new Workshop
        {
            Nome = request.Nome,
            DataRealizacao = request.DataRealizacao,
            Descricao = request.Descricao
        };

        await _workshopRepository.AddAsync(workshop);

        return MapToResponse(workshop);
    }

    public async Task<WorkshopResponseDto> GetWorkshopAsync(int id)
    {
        var workshop = await _workshopRepository.GetByIdAsync(id);
        if (workshop is null)
        {
            throw new EntityNotFoundException($"Workshop {id} não encontrado.");
        }

        return MapToResponse(workshop);
    }

    private static WorkshopResponseDto MapToResponse(Workshop workshop)
    {
        return new WorkshopResponseDto
        {
            Id = workshop.Id,
            Nome = workshop.Nome,
            DataRealizacao = workshop.DataRealizacao,
            Descricao = workshop.Descricao
        };
    }
}
