using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using System.IO;

// Load environment variables from .env file
DotNetEnv.Env.Load("../.env");

var builder = WebApplication.CreateBuilder(args);

// ========================================
// LAYER DEPENDENCY INJECTION
// ========================================

// Add Application layer services (business logic, DTOs, interfaces)
builder.Services.AddApplication();

// Add Infrastructure layer services (Database context, Identity, Authentication)
builder.Services.AddInfrastructure(builder.Configuration);

// ========================================
// AUTHENTICATION CONFIGURATION
// ========================================

// Authentication is configured in Infrastructure layer via AddIdentity
// Do NOT add AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) here
// as it conflicts with Identity's default scheme ("Identity.Application")

// Data Protection for cookie encryption, key persistence
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/app/keys")) // Ensure folder exists in Docker
    .SetApplicationName("FrontEnd.Web");

// ========================================
// PRESENTATION LAYER SERVICES
// ========================================

// Add Razor Pages services
builder.Services.AddRazorPages();

// Optional: HttpContext accessor for accessing user info in services
builder.Services.AddHttpContextAccessor();

// ========================================
// BUILD AND CONFIGURE PIPELINE
// ========================================
var app = builder.Build();

// ========================================
// SEED DATABASE
// ========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // Seed database with initial data (e.g., admin user)
    await Infrastructure.DependencyInjection.SeedDatabaseAsync(services);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map Razor Pages routes
app.MapRazorPages();

// Run the application
app.Run();
