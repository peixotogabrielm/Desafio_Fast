using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Desafio_Fast.Data;
using Desafio_Fast.Models;
using Desafio_Fast.DTOs;

namespace Desafio_Fast.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class ColaboradoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ColaboradoresController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<ColaboradorDTO>> CreateColaborador(CreateColaboradorDTO createColaboradorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingColaborador = await _context.Colaboradores
                .FirstOrDefaultAsync(c => c.Nome.ToLower() == createColaboradorDto.Nome.ToLower());

            if (existingColaborador != null)
                return BadRequest($"Já existe um colaborador com o nome '{createColaboradorDto.Nome}'.");

            var colaborador = new Colaborador
            {
                Nome = createColaboradorDto.Nome
            };

            _context.Colaboradores.Add(colaborador);
            await _context.SaveChangesAsync();

            var colaboradorDto = new ColaboradorDTO
            {
                Id = colaborador.Id,
                Nome = colaborador.Nome
            };

            return CreatedAtAction(nameof(GetColaborador), new { id = colaborador.Id }, colaboradorDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ColaboradorDTO>> GetColaborador(int id)
        {
            var colaborador = await _context.Colaboradores
                .Include(c => c.AtaColaboradores)
                    .ThenInclude(ac => ac.Ata)
                        .ThenInclude(a => a.Workshop)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (colaborador == null)
                return NotFound($"Colaborador com ID {id} não encontrado.");

            var colaboradorDto = new ColaboradorDTO
            {
                Id = colaborador.Id,
                Nome = colaborador.Nome,
                Workshops = colaborador.AtaColaboradores
                    .Select(ac => new WorkshopResumoDTO
                    {
                        Id = ac.Ata.Workshop.Id,
                        Nome = ac.Ata.Workshop.Nome,
                        DataRealizacao = ac.Ata.Workshop.DataRealizacao
                    })
                    .OrderBy(w => w.DataRealizacao)
                    .ToList()
            };

            return colaboradorDto;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColaboradorDTO>>> GetColaboradores()
        {
            var colaboradores = await _context.Colaboradores
                .Include(c => c.AtaColaboradores)
                    .ThenInclude(ac => ac.Ata)
                        .ThenInclude(a => a.Workshop)
                .OrderBy(c => c.Nome)
                .Select(c => new ColaboradorDTO
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Workshops = c.AtaColaboradores
                        .Select(ac => new WorkshopResumoDTO
                        {
                            Id = ac.Ata.Workshop.Id,
                            Nome = ac.Ata.Workshop.Nome,
                            DataRealizacao = ac.Ata.Workshop.DataRealizacao
                        })
                        .OrderBy(w => w.DataRealizacao)
                        .ToList()
                })
                .ToListAsync();

            return colaboradores;
        }
    }
}