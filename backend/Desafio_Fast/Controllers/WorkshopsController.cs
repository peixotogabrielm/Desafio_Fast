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

    public class WorkshopsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkshopsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<WorkshopDTO>> CreateWorkshop(CreateWorkshopDTO createWorkshopDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workshop = new Workshop
            {
                Nome = createWorkshopDto.Nome,
                DataRealizacao = createWorkshopDto.DataRealizacao,
                Descricao = createWorkshopDto.Descricao
            };

            _context.Workshops.Add(workshop);
            await _context.SaveChangesAsync();

            var workshopDto = new WorkshopDTO
            {
                Id = workshop.Id,
                Nome = workshop.Nome,
                DataRealizacao = workshop.DataRealizacao,
                Descricao = workshop.Descricao
            };

            return CreatedAtAction(nameof(GetWorkshop), new { id = workshop.Id }, workshopDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkshopDTO>> GetWorkshop(int id)
        {
            var workshop = await _context.Workshops.FindAsync(id);

            if (workshop == null)
                return NotFound($"Workshop com ID {id} não encontrado.");

            var workshopDto = new WorkshopDTO
            {
                Id = workshop.Id,
                Nome = workshop.Nome,
                DataRealizacao = workshop.DataRealizacao,
                Descricao = workshop.Descricao
            };

            return workshopDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkshopDTO>>> GetWorkshops()
        {
            var workshops = await _context.Workshops
                .OrderBy(w => w.DataRealizacao)
                .Select(w => new WorkshopDTO
                {
                    Id = w.Id,
                    Nome = w.Nome,
                    DataRealizacao = w.DataRealizacao,
                    Descricao = w.Descricao
                })
                .ToListAsync();

            return workshops;
        }
    }
}