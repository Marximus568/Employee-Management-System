using DotNetEnv;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

/// <summary>
/// Factory used by EF Core tools at design-time to create the ApplicationDbContext.
/// This ensures migrations can run even when dependency injection is not available.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <summary>
    /// Creates a new instance of ApplicationDbContext using the connection string
    /// from the environment variable DB_CONNECTION.
    /// </summary>
    /// <param name="args">Command line arguments (ignored).</param>
    /// <returns>An instance of ApplicationDbContext configured for PostgreSQL.</returns>
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // 1. Try to find .env in current directory
        Env.Load();

        // 2. If DB_CONNECTION is still null, try finding .env in the solution root
        // We assume the solution root is where the .sln usually is, or just go up levels
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_CONNECTION")))
        {
            var current = Directory.GetCurrentDirectory();
            // Try going up a few levels to find .env
            for (int i = 0; i < 5; i++)
            {
                var parent = Directory.GetParent(current)?.FullName;
                if (parent == null) break;
                
                var envPath = Path.Combine(parent, ".env");
                if (File.Exists(envPath))
                {
                    Env.Load(envPath);
                    break;
                }
                current = parent;
            }
        }
        
        // 3. Fallback: Hard path for this specific environment if simple traversal fails
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_CONNECTION")))
        {
             var fixedPath = "/home/Coder/RiderProjects/V_Express_Firmeza/.env";
             if(File.Exists(fixedPath)) Env.Load(fixedPath);
        }

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Read the connection string from the environment variable
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException(
                "DB_CONNECTION environment variable is not set. Make sure you have a .env file with DB_CONNECTION in the solution root.");

        // Configure the DbContext to use PostgreSQL
        optionsBuilder.UseNpgsql(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}