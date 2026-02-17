using ys.api.properties.Dtos.Property;
using ys.api.properties.Models;
using ys.api.properties.Models.Results;

namespace ys.api.properties.Interfaces;

public interface IOverlapRepository
{
    public Task<RepositoryResult<List<PropertyModelEntity>>> GetPropertiesOverlappingAsync(OverlapQueryDto query);
}
