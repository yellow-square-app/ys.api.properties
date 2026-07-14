using ys.api.properties.Models;
using ys.api.properties.Models.Results;

namespace ys.api.properties.Interfaces;

public interface IAddressDataRepository
{
    public Task<RepositoryResult<List<CountryDataEntity>>> GetCountryDataAsync();
    public Task<RepositoryResult<CountryDataEntity>> GetCountryDataByCodeAsync(string countryCode);
    public Task<RepositoryResult<List<RegionEntity>>> GetRegionsAsync(string countryCode);
    public Task<RepositoryResult<List<RegionEntity>>> GetSubRegionsAsync(string countryCode);
}
