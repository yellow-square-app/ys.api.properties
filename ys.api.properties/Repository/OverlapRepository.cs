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
/// Repository for performing polygon overlap/intersection spatial queries on Property entities.
/// Uses PostGIS ST_Contains for point-in-polygon and ST_Intersects for polygon boundary overlap.
/// </summary>
/// <param name="context">The application database context</param>
/// <param name="logger">Logger for the OverlapRepository</param>
public class OverlapRepository(
    ApplicationDbContext context,
    ILogger<OverlapRepository> logger
) : IOverlapRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<OverlapRepository> _logger = logger;

    /// <summary>
    /// Retrieves all properties whose location or boundary overlaps with the specified polygon.
    /// Uses NTS LINQ which translates to PostGIS:
    /// - ST_Contains(polygon, location) for point locations
    /// - ST_Intersects(boundary, polygon) for polygon boundaries
    /// Results are deduplicated by property ID.
    /// </summary>
    /// <param name="query">The overlap query containing polygon coordinate pairs [lng, lat]</param>
    /// <returns>A repository result containing a deduplicated list of overlapping properties</returns>
    public async Task<RepositoryResult<List<PropertyModelEntity>>> GetPropertiesOverlappingAsync(OverlapQueryDto query)
    {
        _logger.LogInformation(Messages.Action.Called(nameof(GetPropertiesOverlappingAsync)));

        // Build the polygon from coordinate pairs [lng, lat]
        var geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);

        // Ensure the polygon is closed (first point == last point)
        var coordinates = query.Coordinates
            .Select(c => new Coordinate(c[0], c[1]))
            .ToList();

        if (coordinates.Count < 3)
        {
            return RepositoryResult<List<PropertyModelEntity>>.FailureResult(new Exception(Messages.Geo.InvalidPolygon));
        }

        // Close the ring if not already closed
        if (!coordinates.First().Equals2D(coordinates.Last()))
        {
            coordinates.Add(coordinates.First());
        }

        var polygon = geometryFactory.CreatePolygon(coordinates.ToArray());

        // Query for properties whose point location is contained within the polygon (ST_Contains)
        var pointResults = await _context.Properties
            .Where(p => p.location != null && polygon.Contains(p.location))
            .ToListAsync();

        // Query for properties whose polygon boundary intersects with the query polygon (ST_Intersects)
        var boundaryResults = await _context.Properties
            .Where(p => p.boundary != null && p.boundary.Intersects(polygon))
            .ToListAsync();

        // Deduplicate by property ID
        var combined = pointResults
            .Union(boundaryResults)
            .DistinctBy(p => p.id)
            .ToList();

        if (!combined.Any())
        {
            _logger.LogWarning(Messages.Geo.NoPropertiesInArea);
            return RepositoryResult<List<PropertyModelEntity>>.FailureResult(new Exception(Messages.Geo.NoPropertiesInArea));
        }

        _logger.LogInformation(Messages.Success.Found($"{combined.Count} overlapping properties"));
        return RepositoryResult<List<PropertyModelEntity>>.SuccessResult(combined, Messages.Success.Found($"{combined.Count} properties"));
    }
}
