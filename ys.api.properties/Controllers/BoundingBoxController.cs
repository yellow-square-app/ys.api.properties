using System.Net;
using Microsoft.AspNetCore.Mvc;
using ys.api.properties.Constants;
using ys.api.properties.Dtos.Property;
using ys.api.properties.Interfaces;
using ys.api.properties.Models;

namespace ys.api.properties.Controllers
{
    /// <summary>
    /// Controller for bounding box spatial queries on properties.
    /// Uses PostGIS ST_Contains(ST_MakeEnvelope(...), location) for spatial filtering.
    /// </summary>
    [Route("api/geo/boundingbox")]
    [ApiController]
    public class BoundingBoxController : ControllerBase
    {
        private readonly IBoundingBoxRepository _boundingBoxRepository;
        private readonly ILogger<BoundingBoxController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingBoxController"/> class.
        /// </summary>
        /// <param name="boundingBoxRepository">The bounding box repository.</param>
        /// <param name="logger">The logger instance.</param>
        public BoundingBoxController(IBoundingBoxRepository boundingBoxRepository, ILogger<BoundingBoxController> logger)
        {
            _boundingBoxRepository = boundingBoxRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all properties within the specified bounding box.
        /// </summary>
        /// <param name="query">The bounding box query parameters (minLat, minLng, maxLat, maxLng).</param>
        /// <returns>A list of properties within the bounding box.</returns>
        [HttpPost("getPropertiesInBoundingBox")]
        [ProducesResponseType(typeof(List<PropertyModelEntity>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPropertiesInBoundingBox([FromBody] BoundingBoxQueryDto query)
        {
            if (query.MinLat >= query.MaxLat || query.MinLng >= query.MaxLng)
                return BadRequest(Messages.Geo.InvalidBoundingBox);

            var result = await _boundingBoxRepository.GetPropertiesInBoundingBoxAsync(query);

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result?.Data);
        }
    }
}
