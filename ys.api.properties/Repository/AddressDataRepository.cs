using Microsoft.EntityFrameworkCore;
using ys.api.properties.Constants;
using ys.api.properties.Data;
using ys.api.properties.Interfaces;
using ys.api.properties.Models;
using ys.api.properties.Models.Results;

namespace ys.api.properties.Repository;

/// <summary>
/// Repository for querying address reference data (countries, regions, sub-regions).
/// </summary>
/// <param name="context">The application database context</param>
/// <param name="logger">Logger for the AddressDataRepository</param>
public class AddressDataRepository(
    ApplicationDbContext context,
    ILogger<AddressDataRepository> logger
) : IAddressDataRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<AddressDataRepository> _logger = logger;

    /// <summary>
    /// Retrieves all active country data records.
    /// </summary>
    public async Task<RepositoryResult<List<CountryDataEntity>>> GetCountryDataAsync()
    {
        _logger.LogInformation(Messages.Action.Called(nameof(GetCountryDataAsync)));

        var countries = await _context.CountryData
            .Where(c => c.is_active)
            .OrderBy(c => c.name)
            .ToListAsync();

        if (!countries.Any())
        {
            _logger.LogWarning(Messages.Errors.NotFound("country data"));
            return RepositoryResult<List<CountryDataEntity>>.FailureResult(
                new Exception(Messages.Errors.NotFound("country data")));
        }

        _logger.LogInformation(Messages.Success.Found($"{countries.Count} countries"));
        return RepositoryResult<List<CountryDataEntity>>.SuccessResult(
            countries, Messages.Success.Found($"{countries.Count} countries"));
    }

    /// <summary>
    /// Retrieves a single country by its alpha-2 code.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO 3166-1 alpha-2 code</param>
    public async Task<RepositoryResult<CountryDataEntity>> GetCountryDataByCodeAsync(string countryCode)
    {
        _logger.LogInformation(Messages.Action.Called($"{nameof(GetCountryDataByCodeAsync)}:{countryCode}"));

        var country = await _context.CountryData
            .FirstOrDefaultAsync(c => c.alpha_2_code == countryCode.ToUpper() && c.is_active);

        if (country is null)
        {
            _logger.LogWarning(Messages.Errors.NotFound($"Country {countryCode}"));
            return RepositoryResult<CountryDataEntity>.FailureResult(
                new Exception(Messages.Errors.NotFound($"country with code {countryCode}")));
        }

        _logger.LogInformation(Messages.Success.Found($"Country {countryCode}"));
        return RepositoryResult<CountryDataEntity>.SuccessResult(
            country, Messages.Success.Found($"country {country.name}"));
    }

    /// <summary>
    /// Retrieves all active regions (states, provinces, districts, territories) for a country.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO 3166-1 alpha-2 code</param>
    public async Task<RepositoryResult<List<RegionEntity>>> GetRegionsAsync(string countryCode)
    {
        _logger.LogInformation(Messages.Action.Called($"{nameof(GetRegionsAsync)}:{countryCode}"));

        var regions = await _context.Regions
            .Where(r => r.country_code == countryCode.ToUpper() && r.is_active)
            .OrderBy(r => r.display_name)
            .ToListAsync();

        if (!regions.Any())
        {
            _logger.LogWarning(Messages.Errors.NotFound($"Regions for {countryCode}"));
            return RepositoryResult<List<RegionEntity>>.FailureResult(
                new Exception(Messages.Errors.NotFound($"regions for country {countryCode}")));
        }

        _logger.LogInformation(Messages.Success.Found($"{regions.Count} regions for {countryCode}"));
        return RepositoryResult<List<RegionEntity>>.SuccessResult(
            regions, Messages.Success.Found($"{regions.Count} regions"));
    }

    /// <summary>
    /// Retrieves all active sub-regions (counties, districts) for a country.
    /// Stub: returns results filtered by region_type "Sub-region" — currently no data seeded.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO 3166-1 alpha-2 code</param>
    public async Task<RepositoryResult<List<RegionEntity>>> GetSubRegionsAsync(string countryCode)
    {
        _logger.LogInformation(Messages.Action.Called($"{nameof(GetSubRegionsAsync)}:{countryCode}"));

        var subRegions = await _context.Regions
            .Where(r => r.country_code == countryCode.ToUpper()
                        && r.region_type == "Sub-region"
                        && r.is_active)
            .OrderBy(r => r.display_name)
            .ToListAsync();

        if (!subRegions.Any())
        {
            _logger.LogWarning(Messages.Errors.NotFound($"Sub-regions for {countryCode}"));
            return RepositoryResult<List<RegionEntity>>.FailureResult(
                new Exception(Messages.Errors.NotFound($"sub-regions for country {countryCode}")));
        }

        _logger.LogInformation(Messages.Success.Found($"{subRegions.Count} sub-regions for {countryCode}"));
        return RepositoryResult<List<RegionEntity>>.SuccessResult(
            subRegions, Messages.Success.Found($"{subRegions.Count} sub-regions"));
    }
}
