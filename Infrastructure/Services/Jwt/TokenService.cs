using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Infrastructure.Models;
using Microsoft.Extensions.Options; // Changed from Configuration
using Microsoft.IdentityModel.Tokens;

using Infrastructure.Services.Identity.Interface;
using Application.DTOs.Auth;

namespace Infrastructure.Services.Jwt;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public async Task<AuthResponseDto> GenerateTokensAsync(ApplicationUser user, string ipAddress)
    {
        var jwtToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken(ipAddress);

        // Note: Refresh token persistence should be handled by the caller (AuthService) 
        // or we need to inject User/Context here to save it. 
        // For now, we just return the DTO.

        return new AuthResponseDto
        {
            AccessToken = jwtToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = refreshToken.Expires,
            Email = user.Email,
            UserId = user.Id
        };
    }

    public Task<AuthResponseDto> RefreshTokenAsync(string token, string ipAddress)
    {
        throw new NotImplementedException("RefreshTokenAsync not implemented in TokenService yet.");
    }

    public string GenerateJwtToken(ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim("FirstName", user.FirstName ?? ""),
        };
        
        // Add roles if available (handled by caller usually, but good to have)
        // For now basics are enough.

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(string ipAddress)
    {
        return new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }
}
