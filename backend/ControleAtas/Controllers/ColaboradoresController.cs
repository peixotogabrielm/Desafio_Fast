using ControleAtas.DTOs;
using ControleAtas.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleAtas.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ColaboradoresController : ControllerBase
{
    private readonly IColaboradorService _colaboradorService;

    public ColaboradoresController(IColaboradorService colaboradorService)
    {
        _colaboradorService = colaboradorService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ColaboradorSummaryDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateColaborador([FromBody] ColaboradorCreateDto request)
    {
        var response = await _colaboradorService.CreateColaboradorAsync(request);
        return Created("", response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ColaboradorResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ColaboradorResponseDto>>> GetColaboradores()
    {
        var response = await _colaboradorService.GetColaboradoresAsync();
        return Ok(response);
    }
}
