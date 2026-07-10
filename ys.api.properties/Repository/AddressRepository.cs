using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using ys.api.properties.Constants;
using ys.api.properties.Data;
using ys.api.properties.Dtos.Address;
using ys.api.properties.Interfaces;
using ys.api.properties.Models;
using ys.api.properties.Models.Results;

namespace ys.api.properties.Repository;

/// <summary>
/// Repository for managing Address records in the ys-properties table.
/// </summary>
/// <param name="context">The application database context</param>
/// <param name="logger">Logger for the AddressRepository</param>
public class AddressRepository(
    ApplicationDbContext context,
    ILogger<AddressRepository> logger
) : IAddressRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<AddressRepository> _logger = logger;

    /// <summary>
    /// Retrieves all addresses from the database.
    /// </summary>
    public async Task<RepositoryResult<List<PropertyModelEntity>>> GetAddressesAsync()
    {
        _logger.LogInformation(Messages.Action.Called(nameof(GetAddressesAsync)));

        var addresses = await _context.Properties.ToListAsync();

        if (!addresses.Any())
        {
            _logger.LogWarning(Messages.Errors.NotFound("addresses"));
            return RepositoryResult<List<PropertyModelEntity>>.FailureResult(new Exception(Messages.Errors.NotFound("addresses")));
        }

        _logger.LogInformation(Messages.Success.Found("addresses"));
        return RepositoryResult<List<PropertyModelEntity>>.SuccessResult(addresses, Messages.Success.Found("addresses"));
    }

    /// <summary>
    /// Retrieves a specific address by its ID.
    /// </summary>
    public async Task<RepositoryResult<PropertyModelEntity>?> GetAddressByIdAsync(Guid addressId)
    {
        _logger.LogInformation(Messages.Action.Called($"{nameof(GetAddressByIdAsync)}:{addressId}"));

        var address = await _context.Properties.FindAsync(addressId);

        if (address is null)
        {
            _logger.LogWarning(Messages.Errors.NotFound($"Address {addressId}"));
            return RepositoryResult<PropertyModelEntity>.FailureResult(new Exception(Messages.Errors.NotFound("address")));
        }

        _logger.LogInformation(Messages.Success.Found($"Address {addressId}"));
        return RepositoryResult<PropertyModelEntity>.SuccessResult(address, Messages.Success.Found($"Address {addressId}"));
    }

    /// <summary>
    /// Retrieves all addresses associated with a given parent ID.
    /// </summary>
    public async Task<RepositoryResult<List<PropertyModelEntity>>> GetAddressesByParentIdAsync(Guid parentId)
    {
        _logger.LogInformation(Messages.Action.Called($"{nameof(GetAddressesByParentIdAsync)}:{parentId}"));

        var addresses = await _context.Properties
            .Where(a => a.parent_id == parentId)
            .ToListAsync();

        if (!addresses.Any())
        {
            _logger.LogWarning(Messages.Errors.NotFound($"Addresses for parent {parentId}"));
            return RepositoryResult<List<PropertyModelEntity>>.FailureResult(new Exception(Messages.Errors.NotFound("addresses")));
        }

        _logger.LogInformation(Messages.Success.Found($"{addresses.Count} addresses for parent {parentId}"));
        return RepositoryResult<List<PropertyModelEntity>>.SuccessResult(addresses, Messages.Success.Found($"{addresses.Count} addresses"));
    }

    /// <summary>
    /// Creates a new address in the database.
    /// </summary>
    public async Task<RepositoryResult<PropertyModelEntity>> CreateAddressAsync(PropertyModelEntity addressEntity)
    {
        _logger.LogInformation(Messages.Action.Called(nameof(CreateAddressAsync)));

        await _context.Properties.AddAsync(addressEntity);
        await _context.SaveChangesAsync();

        _logger.LogInformation(Messages.Success.Created($"Address {addressEntity.id}"));
        return RepositoryResult<PropertyModelEntity>.SuccessResult(
            addressEntity,
            Messages.Success.Created($"address {addressEntity.id}")
        );
    }

    /// <summary>
    /// Updates an existing address in the database.
    /// </summary>
    public async Task<RepositoryResult<PropertyModelEntity>?> UpdateAddressAsync(UpdateAddressDto addressDto)
    {
        _logger.LogInformation(Messages.Action.Called(nameof(UpdateAddressAsync)));

        var addressId = addressDto.address_id;

        var address = await _context.Properties.FirstOrDefaultAsync(a => a.id == addressId);

        if (address is null)
        {
            _logger.LogWarning(Messages.Errors.NotFound($"Address {addressId}"));
            return RepositoryResult<PropertyModelEntity>.FailureResult(new Exception(Messages.Errors.NotFound("address")));
        }

        address.name = addressDto.name ?? string.Empty;
        address.description = addressDto.description ?? string.Empty;
        address.address_line_1 = addressDto.address_line_1;
        address.address_line_2 = addressDto.address_line_2;
        address.address_line_3 = addressDto.address_line_3;
        address.address_line_4 = addressDto.address_line_4;
        address.city = addressDto.city;
        address.state = addressDto.state;
        address.region_type = addressDto.region_type;
        address.sub_region_name = addressDto.sub_region_name;
        address.sub_region_type = addressDto.sub_region_type;
        address.postal_code = addressDto.postal_code;
        address.postal_code_type = addressDto.postal_code_type;
        address.country_code = addressDto.country_code;
        address.is_address_verified = addressDto.is_address_verified;
        address.latitude = addressDto.latitude;
        address.longitude = addressDto.longitude;
        address.version += 1;
        address.updated_on = DateTime.UtcNow;

        if (addressDto.latitude.HasValue && addressDto.longitude.HasValue)
        {
            address.location = new Point(addressDto.longitude.Value, addressDto.latitude.Value) { SRID = 4326 };
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation(Messages.Success.Updated($"Address {addressId}"));
        return RepositoryResult<PropertyModelEntity>.SuccessResult(
            address,
            Messages.Success.Updated($"address {addressId}")
        );
    }

    /// <summary>
    /// Deletes an address from the database.
    /// </summary>
    public async Task<RepositoryResult<PropertyModelEntity>?> DeleteAddressAsync(Guid addressId)
    {
        _logger.LogInformation(Messages.Action.Called(nameof(DeleteAddressAsync)));

        var address = await _context.Properties.FirstOrDefaultAsync(a => a.id == addressId);

        if (address is null)
        {
            _logger.LogWarning(Messages.Errors.NotFound($"Address {addressId}"));
            return RepositoryResult<PropertyModelEntity>.FailureResult(new Exception(Messages.Errors.NotFound("address")));
        }

        _context.Properties.Remove(address);
        await _context.SaveChangesAsync();

        _logger.LogInformation(Messages.Success.Deleted($"Address {addressId}"));
        return RepositoryResult<PropertyModelEntity>.SuccessResult(
            address,
            Messages.Success.Deleted($"address {addressId}")
        );
    }
}
