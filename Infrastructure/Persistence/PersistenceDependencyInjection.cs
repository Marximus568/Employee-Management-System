using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

/// <summary>
/// Dependency injection configuration for persistence layer (Database)
/// </summary>
public static class PersistenceDependencyInjection
{
    /// <summary>
    /// Adds persistence services (DbContext, migrations, etc.) to the service collection
    /// </summary>
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Get connection string from configuration
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found in configuration. " +
                "Please add it to appsettings.json");

        // Register ApplicationDbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions.MigrationsAssembly("Infrastructure")));

        return services;
    }
}

