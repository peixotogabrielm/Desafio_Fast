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
    public class AtasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AtasController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<AtaDTO>> CreateAta(CreateAtaDTO createAtaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workshop = await _context.Workshops.FindAsync(createAtaDto.WorkshopId);
            if (workshop == null)
                return NotFound($"Workshop com ID {createAtaDto.WorkshopId} não encontrado.");

            var existingAta = await _context.Atas
                .FirstOrDefaultAsync(a => a.WorkshopId == createAtaDto.WorkshopId);

            if (existingAta != null)
                return BadRequest($"Já existe uma ata para o workshop '{workshop.Nome}'.");

            var ata = new Ata
            {
                WorkshopId = createAtaDto.WorkshopId
            };

            _context.Atas.Add(ata);
            await _context.SaveChangesAsync();

            var createdAta = await _context.Atas
                .Include(a => a.Workshop)
                .Include(a => a.AtaColaboradores)
                    .ThenInclude(ac => ac.Colaborador)
                .FirstAsync(a => a.Id == ata.Id);

            var ataDto = new AtaDTO
            {
                Id = createdAta.Id,
                Workshop = new WorkshopDTO
                {
                    Id = createdAta.Workshop.Id,
                    Nome = createdAta.Workshop.Nome,
                    DataRealizacao = createdAta.Workshop.DataRealizacao,
                    Descricao = createdAta.Workshop.Descricao
                },
                Colaboradores = new List<ColaboradorDTO>()
            };

            return CreatedAtAction(nameof(GetAta), new { id = ata.Id }, ataDto);
        }

        [HttpPut("{ataId}/colaboradores/{colaboradorId}")]
        public async Task<IActionResult> AddColaboradorToAta(int ataId, int colaboradorId)
        {
            var ata = await _context.Atas.FindAsync(ataId);
            if (ata == null)
                return NotFound($"Ata com ID {ataId} não encontrada.");

            var colaborador = await _context.Colaboradores.FindAsync(colaboradorId);
            if (colaborador == null)
                return NotFound($"Colaborador com ID {colaboradorId} não encontrado.");

            var existingRelation = await _context.AtaColaboradores
                .FirstOrDefaultAsync(ac => ac.AtaId == ataId && ac.ColaboradorId == colaboradorId);

            if (existingRelation != null)
                return BadRequest($"Colaborador '{colaborador.Nome}' já está presente na ata.");

            var ataColaborador = new AtaColaborador
            {
                AtaId = ataId,
                ColaboradorId = colaboradorId
            };

            _context.AtaColaboradores.Add(ataColaborador);
            await _context.SaveChangesAsync();

            return Ok($"Colaborador '{colaborador.Nome}' adicionado à ata com sucesso.");
        }

        [HttpDelete("{ataId}/colaboradores/{colaboradorId}")]
        public async Task<IActionResult> RemoveColaboradorFromAta(int ataId, int colaboradorId)
        {
            // Verificar se a relação existe
            var ataColaborador = await _context.AtaColaboradores
                .Include(ac => ac.Colaborador)
                .FirstOrDefaultAsync(ac => ac.AtaId == ataId && ac.ColaboradorId == colaboradorId);

            if (ataColaborador == null)
                return NotFound($"Colaborador com ID {colaboradorId} não encontrado na ata {ataId}.");

            _context.AtaColaboradores.Remove(ataColaborador);
            await _context.SaveChangesAsync();

            return Ok($"Colaborador '{ataColaborador.Colaborador.Nome}' removido da ata com sucesso.");
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AtaDTO>> GetAta(int id)
        {
            var ata = await _context.Atas
                .Include(a => a.Workshop)
                .Include(a => a.AtaColaboradores)
                    .ThenInclude(ac => ac.Colaborador)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (ata == null)
                return NotFound($"Ata com ID {id} não encontrada.");

            var ataDto = new AtaDTO
            {
                Id = ata.Id,
                Workshop = new WorkshopDTO
                {
                    Id = ata.Workshop.Id,
                    Nome = ata.Workshop.Nome,
                    DataRealizacao = ata.Workshop.DataRealizacao,
                    Descricao = ata.Workshop.Descricao
                },
                Colaboradores = ata.AtaColaboradores
                    .Select(ac => new ColaboradorDTO
                    {
                        Id = ac.Colaborador.Id,
                        Nome = ac.Colaborador.Nome
                    })
                    .OrderBy(c => c.Nome)
                    .ToList()
            };

            return ataDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AtaComColaboradoresDTO>>> GetAtas(string? workshopNome = null, string? data = null)
        {
            var query = _context.Atas
                .Include(a => a.Workshop)
                .Include(a => a.AtaColaboradores)
                    .ThenInclude(ac => ac.Colaborador)
                .AsQueryable();

            if (!string.IsNullOrEmpty(workshopNome))
            {
                query = query.Where(a => a.Workshop.Nome.ToLower().Contains(workshopNome.ToLower()));
            }

            if (!string.IsNullOrEmpty(data))
            {
                if (DateTime.TryParseExact(data, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime filterDate))
                {
                    query = query.Where(a => a.Workshop.DataRealizacao.Date == filterDate.Date);
                }
                else
                {
                    return BadRequest("Formato de data inválido. Use o formato yyyy-MM-dd (ex: 2024-12-25).");
                }
            }

            var atas = await query
                .OrderBy(a => a.Workshop.DataRealizacao)
                .Select(a => new AtaComColaboradoresDTO
                {
                    Id = a.Id,
                    WorkshopNome = a.Workshop.Nome,
                    DataRealizacao = a.Workshop.DataRealizacao,
                    Colaboradores = a.AtaColaboradores
                        .Select(ac => new ColaboradorDTO
                        {
                            Id = ac.Colaborador.Id,
                            Nome = ac.Colaborador.Nome
                        })
                        .OrderBy(c => c.Nome)
                        .ToList()
                })
                .ToListAsync();

            return atas;
        }
    }
}