namespace Application.DTOs.Auth;

public class EmployeeRegistrationDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    
    // Optional: Employee specific initial fields if any, e.g. Position default?
    // For now, minimal registration as requested.
}
