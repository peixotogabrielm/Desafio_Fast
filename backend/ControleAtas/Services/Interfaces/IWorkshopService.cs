using ControleAtas.DTOs;

namespace ControleAtas.Services.Interfaces;

public interface IWorkshopService
{
    Task<WorkshopResponseDto> CreateWorkshopAsync(WorkshopCreateDto request);
    Task<WorkshopResponseDto> GetWorkshopAsync(int id);
}
