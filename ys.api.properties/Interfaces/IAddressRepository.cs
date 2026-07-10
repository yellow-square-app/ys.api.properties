using ys.api.properties.Dtos.Address;
using ys.api.properties.Models;
using ys.api.properties.Models.Results;

namespace ys.api.properties.Interfaces;

public interface IAddressRepository
{
    public Task<RepositoryResult<List<PropertyModelEntity>>> GetAddressesAsync();
    public Task<RepositoryResult<PropertyModelEntity>?> GetAddressByIdAsync(Guid addressId);
    public Task<RepositoryResult<List<PropertyModelEntity>>> GetAddressesByParentIdAsync(Guid parentId);
    public Task<RepositoryResult<PropertyModelEntity>> CreateAddressAsync(PropertyModelEntity addressEntity);
    public Task<RepositoryResult<PropertyModelEntity>?> UpdateAddressAsync(UpdateAddressDto addressDto);
    public Task<RepositoryResult<PropertyModelEntity>?> DeleteAddressAsync(Guid addressId);
}
