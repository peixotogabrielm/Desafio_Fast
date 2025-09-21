using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Desafio_Fast.Services;
using Desafio_Fast.Data;
using Desafio_Fast.Models;
using Desafio_Fast.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Desafio_Fast.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;
        private readonly ApplicationDbContext _context;

        public AuthController(ITokenService tokenService, IPasswordService passwordService, ApplicationDbContext context)
        {
            _tokenService = tokenService;
            _passwordService = passwordService;
            _context = context;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterUserDTO registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar se username já existe
            var existingUsername = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username.ToLower() == registerDto.Username.ToLower());

            if (existingUsername != null)
                return BadRequest("Username já está em uso.");

            // Verificar se email já existe
            var existingEmail = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == registerDto.Email.ToLower());

            if (existingEmail != null)
                return BadRequest("Email já está em uso.");

            // Criar novo usuário
            var usuario = new Usuario
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = _passwordService.HashPassword(registerDto.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var userDto = new UserDTO
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Email = usuario.Email,
                CreatedAt = usuario.CreatedAt,
                IsActive = usuario.IsActive
            };

            return CreatedAtAction(nameof(GetUser), new { id = usuario.Id }, userDto);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => 
                    (u.Username.ToLower() == loginRequest.UsernameOrEmail.ToLower() ||
                     u.Email.ToLower() == loginRequest.UsernameOrEmail.ToLower()) &&
                    u.IsActive);

            if (usuario == null)
                return Unauthorized("Credenciais inválidas.");

            if (!_passwordService.VerifyPassword(loginRequest.Password, usuario.PasswordHash))
                return Unauthorized("Credenciais inválidas.");

            var token = _tokenService.GenerateToken(usuario.Username);
            return Ok(new { 
                token, 
                expires = DateTime.UtcNow.AddHours(1),
                user = new UserDTO
                {
                    Id = usuario.Id,
                    Username = usuario.Username,
                    Email = usuario.Email,
                    CreatedAt = usuario.CreatedAt,
                    IsActive = usuario.IsActive
                }
            });
        }


        [HttpGet("user/{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            var userDto = new UserDTO
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Email = usuario.Email,
                CreatedAt = usuario.CreatedAt,
                IsActive = usuario.IsActive
            };

            return userDto;
        }

        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var username = User.Identity?.Name;
            
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (usuario == null)
                return Unauthorized();

            var userDto = new UserDTO
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Email = usuario.Email,
                CreatedAt = usuario.CreatedAt,
                IsActive = usuario.IsActive
            };

            return Ok(userDto);
        }
    }

}