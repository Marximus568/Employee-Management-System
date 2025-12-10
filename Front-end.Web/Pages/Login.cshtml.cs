using Application.DTOs.Auth;
using Application.Interfaces.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Front_end.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly Microsoft.AspNetCore.Identity.SignInManager<Infrastructure.Models.ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(
            Microsoft.AspNetCore.Identity.SignInManager<Infrastructure.Models.ApplicationUser> signInManager,
            ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class LoginInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Identity.IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Use SignInManager to sign in
                // This automatically handles password hashing, lockout, and cookie creation with correct claims (including SecurityStamp)
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, isPersistent: true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    
                    // Check if user is admin to redirect to dashboard
                    var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
                    var roles = await _signInManager.UserManager.GetRolesAsync(user);
                    
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToPage("/Dashboard/Index");
                    }
                    
                    return LocalRedirect(returnUrl);
                }
                
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = true });
                }
                
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
