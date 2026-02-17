using System.Net;
using Microsoft.AspNetCore.Mvc;
using ys.api.properties.Constants;
using ys.api.properties.Dtos.Property;
using ys.api.properties.Interfaces;
using ys.api.properties.Models;

namespace ys.api.properties.Controllers
{
    /// <summary>
    /// Controller for polygon overlap/intersection spatial queries on properties.
    /// Uses PostGIS ST_Contains for point-in-polygon and ST_Intersects for polygon boundary overlap.
    /// </summary>
    [Route("api/geo/overlap")]
    [ApiController]
    public class OverlapController : ControllerBase
    {
        private readonly IOverlapRepository _overlapRepository;
        private readonly ILogger<OverlapController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverlapController"/> class.
        /// </summary>
        /// <param name="overlapRepository">The overlap repository.</param>
        /// <param name="logger">The logger instance.</param>
        public OverlapController(IOverlapRepository overlapRepository, ILogger<OverlapController> logger)
        {
            _overlapRepository = overlapRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all properties that overlap with the specified polygon.
        /// Accepts a list of coordinate pairs [longitude, latitude] defining the polygon.
        /// </summary>
        /// <param name="query">The overlap query containing polygon coordinate pairs.</param>
        /// <returns>A deduplicated list of overlapping properties.</returns>
        [HttpPost("getPropertiesOverlapping")]
        [ProducesResponseType(typeof(List<PropertyModelEntity>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPropertiesOverlapping([FromBody] OverlapQueryDto query)
        {
            if (query.Coordinates == null || query.Coordinates.Count < 3)
                return BadRequest(Messages.Geo.InvalidPolygon);

            if (query.Coordinates.Any(c => c == null || c.Length != 2))
                return BadRequest(Messages.Geo.InvalidPolygon);

            var result = await _overlapRepository.GetPropertiesOverlappingAsync(query);

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result?.Data);
        }
    }
}
