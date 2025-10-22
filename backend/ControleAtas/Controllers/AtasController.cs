using ControleAtas.DTOs;
using ControleAtas.Services.Exceptions;
using ControleAtas.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleAtas.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AtasController : ControllerBase
{
    private readonly IAtaService _ataService;

    public AtasController(IAtaService ataService)
    {
        _ataService = ataService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AtaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAta([FromBody] AtaCreateDto request)
    {
        try
        {
            var response = await _ataService.CreateAtaAsync(request);
            return Created($"", response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message, detalhes = ex.Details });
        }
    }

    [HttpPut("{ataId:int}/colaboradores/{colaboradorId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddColaborador(int ataId, int colaboradorId)
    {
        try
        {
            await _ataService.AddColaboradorAsync(ataId, colaboradorId);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message, detalhes = ex.Details });
        }
    }

    [HttpDelete("{ataId:int}/colaboradores/{colaboradorId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveColaborador(int ataId, int colaboradorId)
    {
        try
        {
            await _ataService.RemoveColaboradorAsync(ataId, colaboradorId);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message, detalhes = ex.Details });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AtaResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AtaResponseDto>>> GetAtas([FromQuery] string? workshopNome, [FromQuery] DateTime? data)
    {
        var response = await _ataService.GetAtasAsync(workshopNome, data);
        return Ok(response);
    }
}
