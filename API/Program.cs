using DotNetEnv;
using Infrastructure;
using Infrastructure.Persistence.Extensions;
using Microsoft.OpenApi.Models;

// Load environment variables from .env file
DotNetEnv.Env.Load("../.env");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();       
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Employee Management API",
        Version = "v1"
    });

    // Configure JWT Bearer authentication for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Add Infrastructure, Application and Domain layers
builder.Services.AddInfrastructure(builder.Configuration);
// Automapper Configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply database migrations and seed data on startup
try
{
    await app.Services.ApplyMigrationsAndSeedAsync();
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while applying migrations or seeding the database.");
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map attribute-based controllers
app.MapControllers();                       

app.Run();