using ControleAtas.Models;

namespace ControleAtas.Repositories.Interfaces;

public interface IColaboradorRepository
{
    Task AddAsync(Colaborador colaborador);
    Task<Colaborador?> GetByIdAsync(int id);
    Task<List<Colaborador>> GetByIdsAsync(IEnumerable<int> ids);
    Task<List<Colaborador>> GetAllWithAtasAsync();
}
