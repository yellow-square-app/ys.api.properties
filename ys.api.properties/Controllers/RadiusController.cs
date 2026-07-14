using System.Net;
using Microsoft.AspNetCore.Mvc;
using ys.api.properties.Constants;
using ys.api.properties.Dtos.Property;
using ys.api.properties.Interfaces;
using ys.api.properties.Models;

namespace ys.api.properties.Controllers
{
    /// <summary>
    /// Controller for radius-based spatial queries on properties.
    /// Uses PostGIS ST_DWithin with geography cast for meter-based distance calculation.
    /// </summary>
    [Route("api/geo/radius")]
    [ApiController]
    public class RadiusController : ControllerBase
    {
        private readonly IRadiusRepository _radiusRepository;
        private readonly ILogger<RadiusController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadiusController"/> class.
        /// </summary>
        /// <param name="radiusRepository">The radius repository.</param>
        /// <param name="logger">The logger instance.</param>
        public RadiusController(IRadiusRepository radiusRepository, ILogger<RadiusController> logger)
        {
            _radiusRepository = radiusRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all properties within the specified radius (in meters) of the given point.
        /// </summary>
        /// <param name="query">The radius query parameters (lat, lng, radiusMeters).</param>
        /// <returns>A list of properties within the radius.</returns>
        [HttpPost("get-properties-in-radius")]
        [ProducesResponseType(typeof(List<PropertyModelEntity>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPropertiesInRadius([FromBody] RadiusQueryDto query)
        {
            if (query.RadiusMeters <= 0)
                return BadRequest(Messages.Geo.InvalidRadius);

            if (query.Lat < -90 || query.Lat > 90 || query.Lng < -180 || query.Lng > 180)
                return BadRequest(Messages.Geo.InvalidRadius);

            var result = await _radiusRepository.GetPropertiesInRadiusAsync(query);

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result?.Data);
        }
    }
}
