using Application.Interfaces;
using Application.Interfaces.Identity;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IAuthenticationService = Application.Interfaces.Identity.IAuthenticationService;

namespace Front_end.Web.Pages.Dashboard;

[Authorize]
public class IndexModel : PageModel
{
    private readonly Application.Interfaces.Identity.IAuthenticationService _authService;
    private readonly ApplicationDbContext _context;

    public string UserName { get; set; } = "User";
    public string UserRole { get; set; } = "User";
    
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int InactiveUsers { get; set; }

    public IndexModel(
        Application.Interfaces.Identity.IAuthenticationService authService,
        ApplicationDbContext context)
    {
        _authService = authService;
        _context = context;
    }

    public async Task OnGetAsync()
    {
        var user = await _authService.GetCurrentUserAsync();
        if (user != null)
        {
            UserName = user.FullName;
            UserRole = user.Role;
        }

        // Fetch Employee Stats (not Identity Users)
        TotalUsers = await _context.Employees.CountAsync();
        ActiveUsers = await _context.Employees.CountAsync(e => e.Status == "Active");
        InactiveUsers = await _context.Employees.CountAsync(e => e.Status != "Active");
    }
}
