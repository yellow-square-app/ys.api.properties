using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using ys.api.properties.Constants;
using ys.api.properties.Data;
using ys.api.properties.Dtos.Property;
using ys.api.properties.Interfaces;
using ys.api.properties.Models;
using ys.api.properties.Models.Results;

namespace ys.api.properties.Repository;

/// <summary>
/// Repository for performing bounding box spatial queries on Property entities.
/// Uses PostGIS ST_Contains(ST_MakeEnvelope(...), location) via NTS LINQ translation.
/// </summary>
/// <param name="context">The application database context</param>
/// <param name="logger">Logger for the BoundingBoxRepository</param>
public class BoundingBoxRepository(
    ApplicationDbContext context,
    ILogger<BoundingBoxRepository> logger
) : IBoundingBoxRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<BoundingBoxRepository> _logger = logger;

    /// <summary>
    /// Retrieves all properties whose location falls within the specified bounding box.
    /// Translates to PostGIS: ST_Contains(ST_MakeEnvelope(minLng, minLat, maxLng, maxLat, 4326), location)
    /// </summary>
    /// <param name="query">The bounding box query parameters</param>
    /// <returns>A repository result containing a list of properties within the bounding box</returns>
    public async Task<RepositoryResult<List<PropertyModelEntity>>> GetPropertiesInBoundingBoxAsync(BoundingBoxQueryDto query)
    {
        _logger.LogInformation(Messages.Action.Called(nameof(GetPropertiesInBoundingBoxAsync)));

        // Create an envelope (bounding box) geometry using NTS
        // NTS Envelope.Contains translates to ST_Contains(ST_MakeEnvelope(...), location) in PostGIS
        var envelope = new Envelope(query.MinLng, query.MaxLng, query.MinLat, query.MaxLat);
        var geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
        var boundingBox = geometryFactory.ToGeometry(envelope);

        var properties = await _context.Properties
            .Where(p => p.location != null && boundingBox.Contains(p.location))
            .ToListAsync();

        if (!properties.Any())
        {
            _logger.LogWarning(Messages.Geo.NoPropertiesInArea);
            return RepositoryResult<List<PropertyModelEntity>>.FailureResult(new Exception(Messages.Geo.NoPropertiesInArea));
        }

        _logger.LogInformation(Messages.Success.Found($"{properties.Count} properties in bounding box"));
        return RepositoryResult<List<PropertyModelEntity>>.SuccessResult(properties, Messages.Success.Found($"{properties.Count} properties"));
    }
}
