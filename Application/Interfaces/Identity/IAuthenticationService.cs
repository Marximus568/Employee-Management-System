using Application.DTOs.Auth;

namespace Application.Interfaces.Identity;

/// <summary>
/// Interface for authentication operations
/// </summary>
public interface IAuthenticationService // Renaming for consistency with Frontend usage
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> RegisterEmployeeAsync(EmployeeRegistrationDto request);
    Task LoginAsync(LoginDto request);
    Task RefreshTokenAsync(RefreshTokenRequestDto request);
    Task RevokeTokenAsync(string token);
    Task ConfirmEmailAsync(string userId, string token);
    Task ForgotPasswordAsync(string email);
    Task ResetPasswordAsync(string email, string token, string newPassword);
}
