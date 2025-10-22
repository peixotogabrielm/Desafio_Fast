using ControleAtas.DTOs;

namespace ControleAtas.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(AuthRequestDto request);
        Task<string> Registrar(RegistrarRequestDto request);
    }
}
