using ControleAtas.Data;
using ControleAtas.Models;
using ControleAtas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleAtas.Repositories;

public class ColaboradorRepository : IColaboradorRepository
{
    private readonly ControleAtasContext _context;

    public ColaboradorRepository(ControleAtasContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Colaborador colaborador)
    {
        await _context.Colaboradores.AddAsync(colaborador);
        await _context.SaveChangesAsync();
    }

    public async Task<Colaborador?> GetByIdAsync(int id)
    {
        return await _context.Colaboradores.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Colaborador>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.ToList();
        if (idList.Count == 0)
        {
            return new List<Colaborador>();
        }

        return await _context.Colaboradores
            .Where(c => idList.Contains(c.Id))
            .ToListAsync();
    }

    public async Task<List<Colaborador>> GetAllWithAtasAsync()
    {
        return await _context.Colaboradores
            .AsNoTracking()
            .Include(c => c.Atas)
            .ThenInclude(a => a.Workshop)
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }
}
