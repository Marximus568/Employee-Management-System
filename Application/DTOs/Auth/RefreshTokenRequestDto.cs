namespace Application.DTOs.Auth;

public class RefreshTokenRequestDto
{
    // The user requesting the refresh
    public string UserId { get; set; } = string.Empty;
    
    // The refresh token provided by the client
    public string RefreshToken { get; set; } = string.Empty;
}