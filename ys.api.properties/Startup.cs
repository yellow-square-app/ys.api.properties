using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NetTopologySuite.IO.Converters;
using ys.api.properties.Data;
using ys.api.properties.Interfaces;
using ys.api.properties.Repository;
using ys.api.properties.Helpers;

namespace ys.api.properties;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var postgresUrlVar = Environment.GetEnvironmentVariable("POSTGRES_URL");
        var fallbackConnection = Configuration.GetConnectionString("PropertiesDatabase");
 
        Console.WriteLine($"POSTGRES_URL env var is {(postgresUrlVar != null ? "SET" : "NOT SET")}");
        if (postgresUrlVar != null)
        {
            Console.WriteLine($"  POSTGRES_URL length: {postgresUrlVar.Length}");
            var preview = postgresUrlVar.Length > 30 ? postgresUrlVar.Substring(0, 30) + "..." : postgresUrlVar;
            if (preview.Contains(":"))
            {
                var protocolEnd = preview.IndexOf("://", StringComparison.Ordinal);
                var colonPos = protocolEnd >= 0 ? preview.IndexOf(":", protocolEnd + 3, StringComparison.Ordinal) : -1;
                if (colonPos > 0)
                {
                    preview = preview.Substring(0, colonPos + 1) + "****";
                }
            }
            Console.WriteLine($"  POSTGRES_URL preview: {preview}");
        }

        Console.WriteLine($"Fallback connection from config is {(string.IsNullOrEmpty(fallbackConnection) ? "NOT SET" : "SET")}");

        // Use the first non-null environment variable, prioritizing POSTGRES_CONNECTION
        string? rawConnectionString = null;
        string source = "";
 
        if (!string.IsNullOrWhiteSpace(postgresUrlVar))
        {
            rawConnectionString = postgresUrlVar;
            source = "POSTGRES_URL";
        }
        else if (!string.IsNullOrWhiteSpace(fallbackConnection))
        {
            rawConnectionString = fallbackConnection;
            source = "appsettings.json";
        }

        // Log for debugging (mask password)
        if (string.IsNullOrWhiteSpace(rawConnectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured. Please set POSTGRES_CONNECTION, DATABASE_URL, or POSTGRES_URL environment variable, or ConnectionStrings:PropertiesDatabase in appsettings.json");
        }

        Console.WriteLine($"Using connection string from: {source}");
        Console.WriteLine($"Raw connection string length: {rawConnectionString.Length}");

        // Mask password for logging
        var maskedConnectionString = rawConnectionString;
        Console.WriteLine(rawConnectionString);
        if (rawConnectionString.Contains("://") && rawConnectionString.Contains("@"))
        {
            var atIndex = rawConnectionString.IndexOf('@');
            var colonIndex = rawConnectionString.LastIndexOf(':', atIndex);
            if (colonIndex > 0 && atIndex > colonIndex)
            {
                maskedConnectionString = rawConnectionString.Substring(0, colonIndex + 1) + "****" + rawConnectionString.Substring(atIndex);
            }
        }
        Console.WriteLine($"Raw connection string (masked): {maskedConnectionString}");
        Console.WriteLine($"Raw connection string format detected: {(rawConnectionString.StartsWith("postgresql://") || rawConnectionString.StartsWith("postgres://") ? "URI" : "Standard")}");

        // Convert Railway URI format to standard format if needed
        string connectionString;
        try
        {
            var helpers = new Helpers.Helpers();
            connectionString = helpers.ConvertPostgresUriToConnectionString(rawConnectionString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Failed to convert connection string: {ex.Message}");
            throw;
        }

        // Validate the converted connection string
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Failed to parse database connection string - result was empty");
        }

        Console.WriteLine("Database connection string successfully parsed and validated");

        // Add DbContext with connection resiliency and NetTopologySuite
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    // Enable NetTopologySuite for PostGIS spatial types
                    npgsqlOptions.UseNetTopologySuite();

                    // Enable retry on failure for transient errors
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);

                    // Set command timeout
                    npgsqlOptions.CommandTimeout(60);

                    // Enable connection pooling optimization
                    npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }
            ));

        // Register Helpers for use in controllers/services if needed
        services.AddSingleton<IHelpers, Helpers.Helpers>();

        // Add Repositories
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IBoundingBoxRepository, BoundingBoxRepository>();
        services.AddScoped<IRadiusRepository, RadiusRepository>();
        services.AddScoped<IOverlapRepository, OverlapRepository>();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Add GeoJSON converter for NetTopologySuite geometry serialization
                options.JsonSerializerOptions.Converters.Add(new GeoJsonConverterFactory());
            });
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "jwt_auth", Version = "v1" });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ys.api.properties v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
