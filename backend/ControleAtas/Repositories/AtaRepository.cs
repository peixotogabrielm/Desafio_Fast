using ControleAtas.Data;
using ControleAtas.Models;
using ControleAtas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleAtas.Repositories;

public class AtaRepository : IAtaRepository
{
    private readonly ControleAtasContext _context;

    public AtaRepository(ControleAtasContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Ata ata)
    {
        await _context.Atas.AddAsync(ata);
        await _context.SaveChangesAsync();
    }

    public async Task<Ata?> GetByIdWithColaboradoresAsync(int id)
    {
        return await _context.Atas
            .Include(a => a.Colaboradores)
            .Include(a => a.Workshop)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Ata>> GetAtasAsync(string? workshopNome, DateTime? data)
    {
        var query = _context.Atas
            .AsNoTracking()
            .Include(a => a.Workshop)
            .Include(a => a.Colaboradores)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(workshopNome))
        {
            var normalized = workshopNome.Trim();
            query = query.Where(a => a.Workshop != null && a.Workshop.Nome.Contains(normalized));
        }

        if (data.HasValue)
        {
            var targetDate = data.Value.Date;
            query = query.Where(a => a.Workshop != null && a.Workshop.DataRealizacao.Date == targetDate);
        }

        return await query
            .OrderBy(a => a.Workshop!.DataRealizacao)
            .ThenBy(a => a.Id)
            .ToListAsync();
    }

    public  Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
