using ys.api.properties.Dtos.Property;
using ys.api.properties.Models;
using ys.api.properties.Models.Results;

namespace ys.api.properties.Interfaces;

public interface IPropertyRepository
{
    public Task<RepositoryResult<List<PropertyModelEntity>>> GetPropertiesForUserIdAsync(string userId);
    public Task<RepositoryResult<PropertyModelEntity>?> GetPropertyByIdAsync(Guid propertyId);
    public Task<RepositoryResult<PropertyModelEntity>> CreatePropertyAsync(PropertyModelEntity propertyModelEntity);
    public Task<RepositoryResult<PropertyModelEntity>?> UpdatePropertyAsync(UpdatePropertyDto propertyDto);
    public Task<RepositoryResult<PropertyModelEntity>?> DeletePropertyAsync(Guid propertyId);
    public Task<RepositoryResult<PropertyModelEntity>?> CheckPropertyNameExistsAsync(string propertyName);
}
