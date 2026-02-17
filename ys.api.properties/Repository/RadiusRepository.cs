using Microsoft.EntityFrameworkCore;
using ys.api.properties.Constants;
using ys.api.properties.Data;
using ys.api.properties.Dtos.Property;
using ys.api.properties.Interfaces;
using ys.api.properties.Models;
using ys.api.properties.Models.Results;

namespace ys.api.properties.Repository;

/// <summary>
/// Repository for performing radius-based spatial queries on Property entities.
/// Uses PostGIS ST_DWithin with geography cast for meter-based distance on SRID 4326.
/// </summary>
/// <param name="context">The application database context</param>
/// <param name="logger">Logger for the RadiusRepository</param>
public class RadiusRepository(
    ApplicationDbContext context,
    ILogger<RadiusRepository> logger
) : IRadiusRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<RadiusRepository> _logger = logger;

    /// <summary>
    /// Retrieves all properties whose location is within the specified radius (in meters) of the given point.
    /// Uses raw SQL with PostGIS: ST_DWithin(location::geography, ST_SetSRID(ST_MakePoint(lng, lat), 4326)::geography, radius_meters)
    /// The ::geography cast ensures meter-based distance calculation on SRID 4326.
    /// </summary>
    /// <param name="query">The radius query parameters (lat, lng, radiusMeters)</param>
    /// <returns>A repository result containing a list of properties within the radius</returns>
    public async Task<RepositoryResult<List<PropertyModelEntity>>> GetPropertiesInRadiusAsync(RadiusQueryDto query)
    {
        _logger.LogInformation(Messages.Action.Called(nameof(GetPropertiesInRadiusAsync)));

        var properties = await _context.Properties
            .FromSqlInterpolated($@"
                SELECT * FROM ""ys-properties""
                WHERE location IS NOT NULL
                AND ST_DWithin(
                    location::geography,
                    ST_SetSRID(ST_MakePoint({query.Lng}, {query.Lat}), 4326)::geography,
                    {query.RadiusMeters}
                )")
            .ToListAsync();

        if (!properties.Any())
        {
            _logger.LogWarning(Messages.Geo.NoPropertiesInArea);
            return RepositoryResult<List<PropertyModelEntity>>.FailureResult(new Exception(Messages.Geo.NoPropertiesInArea));
        }

        _logger.LogInformation(Messages.Success.Found($"{properties.Count} properties within {query.RadiusMeters}m radius"));
        return RepositoryResult<List<PropertyModelEntity>>.SuccessResult(properties, Messages.Success.Found($"{properties.Count} properties"));
    }
}
