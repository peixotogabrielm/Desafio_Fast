using ControleAtas.DTOs;
using ControleAtas.Services.Exceptions;
using ControleAtas.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleAtas.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkshopsController : ControllerBase
{
    private readonly IWorkshopService _workshopService;

    public WorkshopsController(IWorkshopService workshopService)
    {
        _workshopService = workshopService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(WorkshopResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateWorkshop([FromBody] WorkshopCreateDto request)
    {
        var response = await _workshopService.CreateWorkshopAsync(request);
        return Created("", new { id = response.Id, response });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(WorkshopResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWorkshop(int id)
    {
        try
        {
            var response = await _workshopService.GetWorkshopAsync(id);
            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
}
