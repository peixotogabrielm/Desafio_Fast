using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ControleAtas.DTOs;
using ControleAtas.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ControleAtas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] AuthRequestDto request)
    {
        try
        {
            var response = await _authService.LoginAsync(request).ConfigureAwait(false);

            return Ok(new AuthResponseDto
            {
                Token = response.Token,
                ExpiresAt = response.ExpiresAt
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Falha de autenticação para o usuário {Email}", request.Email);
            return Unauthorized("Email ou senha inválidos.");
        }catch(Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao autenticar usuário {Email}", request.Email);
            return StatusCode(500, "Erro interno do servidor.");
        }
    }

    [HttpPost("Registrar")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegistrarRequestDto request)
    {
        try
        {
            var response = await _authService.Registrar(request).ConfigureAwait(false);
            return Ok(response);
        }
        catch(InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro ao registrar usuário {Email}", request.Email);
            return BadRequest(ex.Message);
        }catch(Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao registrar usuário {Email}", request.Email);
            return StatusCode(500, "Erro interno do servidor.");
        }

    }
}
