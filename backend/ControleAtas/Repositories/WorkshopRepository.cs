using ControleAtas.Data;
using ControleAtas.Models;
using ControleAtas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleAtas.Repositories;

public class WorkshopRepository : IWorkshopRepository
{
    private readonly ControleAtasContext _context;

    public WorkshopRepository(ControleAtasContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Workshop workshop)
    {
        await _context.Workshops.AddAsync(workshop);
        await _context.SaveChangesAsync();
    }

    public async Task<Workshop?> GetByIdAsync(int id)
    {
        return await _context.Workshops.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
    }
}
