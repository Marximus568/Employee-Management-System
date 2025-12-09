using Application.Interfaces;
using Application.Interfaces.AI;
using Application.Interfaces.Identity;
using Application.Interfaces.SMTP;
using Infrastructure.AI;
using Infrastructure.Models;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.Services.EmployeeService;
using Infrastructure.Services.Identity;
using Infrastructure.Services.Identity.Interface;
using Infrastructure.Services.Jwt;
using Infrastructure.Services.SMTP;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Infrastructure;

/// <summary>
/// Dependency injection configuration for Infrastructure layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ========================================
        // JWT CONFIGURATION
        // ========================================
        services.Configure<Domain.Entities.JwtSettings>(configuration.GetSection("JwtSettings"));

        // ========================================
        // PERSISTENCE CONFIGURATION (Database & Migrations)
        // ========================================
        services.AddPersistence(configuration);

        // ========================================
        // IDENTITY CONFIGURATION
        // ========================================
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 6;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false; // Set to true in production
        })
        .AddEntityFrameworkStores<Persistence.Context.ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Configure cookie authentication
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Login";
            options.LogoutPath = "/Logout";
            options.AccessDeniedPath = "/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
            options.SlidingExpiration = true;
        });

        // ========================================
        // AUTHENTICATION SERVICES
        // ========================================
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        // ========================================
        // AI SERVICES
        // ========================================
        services.AddHttpClient<IAiService, GeminiService>();
        
        services.AddSingleton<IAiService>(serviceProvider =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<GeminiService>>();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();
            
            // Get API key from environment variable
            string? apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "Gemini API Key is not configured. " +
                    "Please add 'GEMINI_API_KEY' to your .env file in the root of your project."
                );
            }
            
            // Optional: Get model name from env (default: gemini-pro)
            string modelName = Environment.GetEnvironmentVariable("GEMINI_MODEL") ?? "gemini-pro";
            
            logger.LogInformation("Gemini service initialized with model: {Model}", modelName);
            
            return new GeminiService(apiKey, logger, httpClient, modelName);
        });

        // ========================================
        // OTHER INFRASTRUCTURE SERVICES
        // ========================================
        
        // SMTP Email Service
        services.Configure<Services.SMTP.SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.AddScoped<Application.Interfaces.SMTP.IEmailService, Services.SMTP.SmtpEmailService>();

        // Domain/Business Services
        services.AddScoped<IEmployeeService, EmployeeService>();

        return services;
    }

    /// <summary>
    /// Seeds the database with initial data
    /// </summary>
    public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
    {
        try
        {
            // Note: This will fail if the database does not exist yet (migrations not applied)
            // We catch the exception to allow the app to start even without the DB
            await IdentitySeeder.SeedAsync(serviceProvider);
        }
        catch (Exception ex)
        {
            var logger = serviceProvider.GetService<ILogger<Program>>();
            // Log as warning instead of error so it doesn't look like a crash
            logger?.LogWarning(ex, "Could not seed database. This is expected if migrations have not been applied yet.");
            
            // Do NOT rethrow the exception, so the app can continue starting
            // throw; 
        }
    }
}
