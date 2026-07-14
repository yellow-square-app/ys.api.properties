using System.Net;
using Microsoft.AspNetCore.Mvc;
using ys.api.properties.Interfaces;
using ys.api.properties.Models;

namespace ys.api.properties.Controllers
{
    /// <summary>
    /// Controller for retrieving address reference data (countries, regions, sub-regions).
    /// Used by the frontend to populate address form dropdowns.
    /// </summary>
    [Route("api/address-data")]
    [ApiController]
    public class AddressDataForCountryController : ControllerBase
    {
        private const string DefaultCountryCode = "US";

        private readonly ILogger<AddressDataForCountryController> _logger;
        private readonly IAddressDataRepository _addressDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressDataForCountryController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="addressDataRepository">The address data repository.</param>
        public AddressDataForCountryController(
            ILogger<AddressDataForCountryController> logger,
            IAddressDataRepository addressDataRepository)
        {
            _logger = logger;
            _addressDataRepository = addressDataRepository;
        }

        /// <summary>
        /// Retrieves all country data records.
        /// </summary>
        /// <returns>A list of all active countries with ISO 3166-1 data.</returns>
        [HttpGet("get-country-data")]
        [ProducesResponseType(typeof(IEnumerable<CountryDataEntity>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCountryData()
        {
            var result = await _addressDataRepository.GetCountryDataAsync();

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result?.Data);
        }

        /// <summary>
        /// Retrieves country data for a specific country code.
        /// </summary>
        /// <param name="countryCode">The 2-letter ISO country code. Defaults to "US".</param>
        /// <returns>The country data record.</returns>
        [HttpGet("get-country-data-by-code")]
        [ProducesResponseType(typeof(CountryDataEntity), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCountryDataByCode([FromQuery] string? countryCode = DefaultCountryCode)
        {
            var code = countryCode ?? DefaultCountryCode;
            var result = await _addressDataRepository.GetCountryDataByCodeAsync(code);

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result?.Data);
        }

        /// <summary>
        /// Retrieves all regions (states, provinces, etc.) for a given country.
        /// </summary>
        /// <param name="countryCode">The 2-letter ISO country code. Defaults to "US".</param>
        /// <returns>A list of regions for the specified country.</returns>
        [HttpGet("get-regions")]
        [ProducesResponseType(typeof(IEnumerable<RegionEntity>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetRegions([FromQuery] string? countryCode = DefaultCountryCode)
        {
            var code = countryCode ?? DefaultCountryCode;
            var result = await _addressDataRepository.GetRegionsAsync(code);

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result?.Data);
        }

        /// <summary>
        /// Retrieves all sub-regions (counties, districts, etc.) for a given country.
        /// Currently stubbed — returns empty until sub-region data is seeded.
        /// </summary>
        /// <param name="countryCode">The 2-letter ISO country code. Defaults to "US".</param>
        /// <returns>A list of sub-regions for the specified country.</returns>
        [HttpGet("get-sub-regions")]
        [ProducesResponseType(typeof(IEnumerable<RegionEntity>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetSubRegions([FromQuery] string? countryCode = DefaultCountryCode)
        {
            var code = countryCode ?? DefaultCountryCode;
            var result = await _addressDataRepository.GetSubRegionsAsync(code);

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result?.Data);
        }
    }
}
