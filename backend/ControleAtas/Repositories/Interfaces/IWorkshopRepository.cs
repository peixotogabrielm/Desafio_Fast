using ControleAtas.Models;

namespace ControleAtas.Repositories.Interfaces;

public interface IWorkshopRepository
{
    Task AddAsync(Workshop workshop);
    Task<Workshop?> GetByIdAsync(int id);
}
