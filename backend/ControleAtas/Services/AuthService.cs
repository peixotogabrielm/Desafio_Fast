using ControleAtas.Data;
using ControleAtas.DTOs;
using ControleAtas.Helpers;
using ControleAtas.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleAtas.Services
{
    public class AuthService : IAuthService
    {
        private readonly ControleAtasContext _context;
        private readonly JwtHelper _jwtHelper;
        public AuthService(ControleAtasContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto> LoginAsync(AuthRequestDto request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email);
            if(usuario == null)
            {
                throw new UnauthorizedAccessException("Email ou senha inválidos.");
            }
            return new AuthResponseDto
            {
                Token = _jwtHelper.GenerateToken(usuario).jwt,
                ExpiresAt = _jwtHelper.GenerateToken(usuario).expire_at
            };
        }

        public async Task<string> Registrar(RegistrarRequestDto request)
        {
            var usuarioExistente = await _context.Usuarios
                .AnyAsync(u => u.Email == request.Email);
            if (usuarioExistente)
            {
                throw new InvalidOperationException("Usuário com este email já existe.");
            }
            var novoUsuario = new Models.Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = request.Password
            };
            var resultado = await _context.Usuarios.AddAsync(novoUsuario);
            await _context.SaveChangesAsync();
            return "Usuário registrado com sucesso.";
        }
    }
}
