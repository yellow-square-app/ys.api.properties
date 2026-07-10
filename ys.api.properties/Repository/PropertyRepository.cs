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
/// Repository for managing Property entities in the database.
/// </summary>
/// <param name="context">The application database context</param>
/// <param name="logger">Logger for the PropertyRepository</param>
public class PropertyRepository(
    ApplicationDbContext context,
    ILogger<PropertyRepository> logger
) : IPropertyRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<PropertyRepository> _logger = logger;

    /// <summary>
    /// Retrieves all properties for a given user ID.
    /// </summary>
    /// <param name="userId">The user ID to filter properties by</param>
    /// <returns>A repository result containing a list of properties or failure information</returns>
    public async Task<RepositoryResult<List<PropertyModelEntity>>> GetPropertiesForUserIdAsync(Guid userId)
    {
        _logger.LogInformation(Messages.Action.Called($"{nameof(GetPropertiesForUserIdAsync)}:{userId}"));

        var properties = await _context.Properties
            .Where(p => p.created_by == userId)
            .ToListAsync();

        if (!properties.Any())
        {
            _logger.LogWarning(Messages.Errors.NotFound($"Properties for user {userId}"));
            return RepositoryResult<List<PropertyModelEntity>>.FailureResult(new Exception(Messages.Errors.NotFound("properties")));
        }

        _logger.LogInformation(Messages.Success.Found($"{properties.Count} properties for user {userId}"));
        return RepositoryResult<List<PropertyModelEntity>>.SuccessResult(properties, Messages.Success.Found($"{properties.Count} properties"));
    }

    /// <summary>
    /// Retrieves a specific property by its ID.
    /// </summary>
    /// <param name="propertyId">The unique identifier of the property</param>
    /// <returns>A repository result containing the property or failure information</returns>
    public async Task<RepositoryResult<PropertyModelEntity>?> GetPropertyByIdAsync(Guid propertyId)
    {
        _logger.LogInformation(Messages.Action.Called($"{nameof(GetPropertyByIdAsync)}:{propertyId}"));

        var property = await _context.Properties.FindAsync(propertyId);

        if (property is null)
        {
            _logger.LogWarning(Messages.Errors.NotFound($"Property {propertyId}"));
            return RepositoryResult<PropertyModelEntity>.FailureResult(new Exception(Messages.Errors.NotFound("property")));
        }

        _logger.LogInformation(Messages.Success.Found($"Property {propertyId}"));
        return RepositoryResult<PropertyModelEntity>.SuccessResult(property, Messages.Success.Found($"Property {propertyId}"));
    }

    /// <summary>
    /// Creates a new property in the database.
    /// </summary>
    /// <param name="propertyModelEntityObj">The property entity to create</param>
    /// <returns>A repository result containing the created property or failure information</returns>
    public async Task<RepositoryResult<PropertyModelEntity>> CreatePropertyAsync(PropertyModelEntity propertyModelEntityObj)
    {
        _logger.LogInformation(Messages.Action.Called(nameof(CreatePropertyAsync)));

        var existingProperty = await _context.Properties
            .FirstOrDefaultAsync(p => p.name == propertyModelEntityObj.name);

        if (existingProperty is not null)
        {
            _logger.LogWarning(Messages.Errors.NameExists($"Property {propertyModelEntityObj.name}"));
            return RepositoryResult<PropertyModelEntity>.FailureResult(new Exception(Messages.Errors.NameExists("property")));
        }

        await _context.Properties.AddAsync(propertyModelEntityObj);
        await _context.SaveChangesAsync();

        _logger.LogInformation(Messages.Success.Created($"Property {propertyModelEntityObj.name}"));
        return RepositoryResult<PropertyModelEntity>.SuccessResult(
            propertyModelEntityObj,
            Messages.Success.Created($"property {propertyModelEntityObj.name}")
        );
    }

    /// <summary>
    /// Updates an existing property in the database.
    /// </summary>
    /// <param name="propertyDto">The updated property data</param>
    /// <returns>A repository result containing the updated property or failure information</returns>
    public async Task<RepositoryResult<PropertyModelEntity>?> UpdatePropertyAsync(UpdatePropertyDto propertyDto)
    {
        _logger.LogInformation(Messages.Action.Called(nameof(UpdatePropertyAsync)));

        var propertyId = propertyDto.PropertyId;

        var property = await _context.Properties.FirstOrDefaultAsync(p => p.id == propertyId);

        if (property is null)
        {
            _logger.LogWarning(Messages.Errors.NotFound($"Property {propertyId}"));
            return RepositoryResult<PropertyModelEntity>.FailureResult(new Exception(Messages.Errors.NotFound("PropertyId")));
        }

        var existingNameCheck = await _context.Properties
            .AnyAsync(p => p.name == propertyDto.Name && p.id != propertyId);

        if (existingNameCheck)
        {
            _logger.LogWarning(Messages.Errors.NameExists($"Property Name {propertyDto.Name}"));
            return RepositoryResult<PropertyModelEntity>.FailureResult(new Exception(Messages.Errors.NameExists("Property")));
        }

        property.name = propertyDto.Name;
        property.description = propertyDto.Description;
        property.version += 1;
        property.updated_on = DateTime.UtcNow;

        if (propertyDto.Latitude.HasValue && propertyDto.Longitude.HasValue)
        {
            property.location = new Point(propertyDto.Longitude.Value, propertyDto.Latitude.Value) { SRID = 4326 };
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation(Messages.Success.Updated($"Property {propertyId}"));
        return RepositoryResult<PropertyModelEntity>.SuccessResult(
            property,
            Messages.Success.Updated($"property {propertyDto.Name}")
        );
    }

    /// <summary>
    /// Deletes a property from the database.
    /// </summary>
    /// <param name="propertyId">The unique identifier of the property to delete</param>
    /// <returns>A repository result containing the deleted property or failure information</returns>
    public async Task<RepositoryResult<PropertyModelEntity>?> DeletePropertyAsync(Guid propertyId)
    {
        _logger.LogInformation(Messages.Action.Called(nameof(DeletePropertyAsync)));

        var property = await _context.Properties.FirstOrDefaultAsync(p => p.id == propertyId);

        if (property is null)
        {
            _logger.LogWarning(Messages.Errors.NotFound($"Property {propertyId}"));
            return RepositoryResult<PropertyModelEntity>.FailureResult(new Exception(Messages.Errors.NotFound("property")));
        }

        _context.Properties.Remove(property);
        await _context.SaveChangesAsync();

        _logger.LogInformation(Messages.Success.Deleted($"Property {propertyId}"));
        return RepositoryResult<PropertyModelEntity>.SuccessResult(
            property,
            Messages.Success.Deleted($"property {property.name}")
        );
    }

    /// <summary>
    /// Checks if a property with the given name exists in the database.
    /// </summary>
    /// <param name="propertyName">The name of the property to check</param>
    /// <returns>A repository result indicating whether the property name exists</returns>
    public async Task<RepositoryResult<PropertyModelEntity>?> CheckPropertyNameExistsAsync(string propertyName)
    {
        _logger.LogInformation(Messages.Action.Called(nameof(CheckPropertyNameExistsAsync)));

        var property = await _context.Properties.FirstOrDefaultAsync(p => p.name == propertyName);

        if (property is null)
        {
            _logger.LogInformation(Messages.Errors.NotFound($"Property with name {propertyName}"));
            return RepositoryResult<PropertyModelEntity>.FailureResult(new Exception(Messages.Errors.NotFound("property")));
        }

        _logger.LogInformation(Messages.Success.Found($"Property with name {propertyName}"));
        return RepositoryResult<PropertyModelEntity>.SuccessResult(property, Messages.Success.Found($"property {propertyName}"));
    }
}
