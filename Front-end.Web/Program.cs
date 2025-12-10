using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

// Load environment variables from .env file
DotNetEnv.Env.Load("../.env");

var builder = WebApplication.CreateBuilder(args);

// ========================================
// LAYER DEPENDENCY INJECTION
// ========================================

// Add Application layer services
builder.Services.AddApplication();

// Cookies Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Path to login page
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });
// Add Infrastructure layer services (Database, Identity, Authentication)
builder.Services.AddInfrastructure(builder.Configuration);

// Data Protection for cookie encryption
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/app/keys"))
    .SetApplicationName("FrontEnd.Web");

// ========================================
// PRESENTATION LAYER SERVICES
// ========================================
builder.Services.AddRazorPages();
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

// Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map Razor Pages
app.MapRazorPages();

app.Run();