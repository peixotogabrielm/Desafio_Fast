using ControleAtas.Models;

namespace ControleAtas.Repositories.Interfaces;

public interface IAtaRepository
{
    Task AddAsync(Ata ata);
    Task<Ata?> GetByIdWithColaboradoresAsync(int id);
    Task<List<Ata>> GetAtasAsync(string? workshopNome, DateTime? data);
    Task SaveChangesAsync();
}
