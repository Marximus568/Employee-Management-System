using Application.DTOs.Auth;
using Application.Interfaces.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front_end.Web.Pages;

public class RegisterModel : PageModel
{
    private readonly IAuthenticationService _authService;

    public RegisterModel(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    public EmployeeRegistrationDto Input { get; set; } = new();

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Input.Password != Input.ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return Page();
        }

        try
        {
            var result = await _authService.RegisterEmployeeAsync(Input);
            SuccessMessage = "Registration successful! Please check your email.";
            // Clearing input to prevent re-submission or show clean state
            Input = new EmployeeRegistrationDto(); 
            return Page();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }
}
