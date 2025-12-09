using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Extensions;

/// <summary>
/// Extension methods for database migrations and initialization
/// </summary>
public static class MigrationExtensions
{
    /// <summary>
    /// Applies pending migrations and initializes the database
    /// </summary>
    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
            
            try
            {
                // Apply any pending migrations
                await dbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }
        }
    }

    /// <summary>
    /// Applies migrations and seeds the database with initial data
    /// </summary>
    public static async Task ApplyMigrationsAndSeedAsync(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
            
            try
            {
                // Apply pending migrations
                await dbContext.Database.MigrateAsync();
                
                // Seed database with initial data
                await DependencyInjection.SeedDatabaseAsync(scope.ServiceProvider);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                throw;
            }
        }
    }
}

