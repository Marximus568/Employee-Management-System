using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using Infrastructure.Services.Identity.Interface;
using Application.DTOs.Auth;

namespace Infrastructure.Services.Jwt;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
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
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim("FirstName", user.FirstName ?? ""),
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
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
