namespace Application.DTOs.Auth;

public class RegisterRequestDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    // Document number required to ensure uniqueness
    public string DocumentNumber { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}