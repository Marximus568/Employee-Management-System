using Application.DTOs.Auth;
using Domain.Entities;
using Infrastructure.Models;

namespace Infrastructure.Services.Identity.Interface;

public interface ITokenService
{
    Task<AuthResponseDto> GenerateTokensAsync(ApplicationUser user, string ipAddress);
    Task<AuthResponseDto> RefreshTokenAsync(string token, string ipAddress);
    string GenerateJwtToken(ApplicationUser user);
    RefreshToken GenerateRefreshToken(string ipAddress);
}