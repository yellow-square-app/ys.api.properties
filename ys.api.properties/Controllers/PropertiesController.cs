using System.Net;
using Microsoft.AspNetCore.Mvc;
using ys.api.properties.Constants;
using ys.api.properties.Data;
using ys.api.properties.Dtos.Property;
using ys.api.properties.Interfaces;
using ys.api.properties.Mappers;
using ys.api.properties.Models;

namespace ys.api.properties.Controllers
{
    /// <summary>
    /// Controller for managing properties.
    /// </summary>
    [Route("api/properties")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly ILogger<PropertiesController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IPropertyRepository _propertyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="context">The database context.</param>
        /// <param name="propertyRepository">The property repository.</param>
        public PropertiesController(ILogger<PropertiesController> logger, ApplicationDbContext context,
            IPropertyRepository propertyRepository)
        {
            _logger = logger;
            _context = context;
            _propertyRepository = propertyRepository;
        }

        /// <summary>
        /// Retrieves all properties.
        /// </summary>
        /// <returns>A list of properties.</returns>
        [HttpGet("getProperties")]
        [ProducesResponseType(typeof(IEnumerable<PropertyModelEntity>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProperties()
        {
            var result = await _propertyRepository.GetPropertiesAsync();

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result?.Data);
        }

        /// <summary>
        /// Retrieves a specific property by its ID.
        /// </summary>
        /// <param name="propertyId">The unique identifier of the property.</param>
        /// <returns>The requested property.</returns>
        [HttpGet("getPropertyById/{propertyId:guid}")]
        [ProducesResponseType(typeof(PropertyModelEntity), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPropertyById(Guid propertyId)
        {
            var result = await _propertyRepository.GetPropertyByIdAsync(propertyId);

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result?.Data);
        }

        /// <summary>
        /// Creates a new property.
        /// </summary>
        /// <param name="propertyDto">The property data to create.</param>
        /// <returns>The created property.</returns>
        [HttpPost("createProperty")]
        [ProducesResponseType(typeof(PropertyModelEntity), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyDto propertyDto)
        {
            var result = await _propertyRepository.CreatePropertyAsync(propertyDto.ToPropertyFromCreatePropertyDto());

            if (result?.Status == Models.Results.StatusCode.Failure)
                return Conflict(Messages.Errors.NameExists("property"));

            return CreatedAtAction(nameof(GetPropertyById), new { propertyId = result?.Data?.id }, result);
        }

        /// <summary>
        /// Updates an existing property.
        /// </summary>
        /// <param name="propertyId">The unique identifier of the property to update.</param>
        /// <param name="propertyDto">The updated property data.</param>
        /// <returns>The updated property.</returns>
        [HttpPut("updateProperty/{propertyId:guid}")]
        [ProducesResponseType(typeof(PropertyModelEntity), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> UpdateProperty(Guid propertyId, [FromBody] UpdatePropertyDto propertyDto)
        {
            if (propertyId != propertyDto.PropertyId)
                return BadRequest("Property ID mismatch");

            var result = await _propertyRepository.UpdatePropertyAsync(propertyDto);

            if (result?.Status == Models.Results.StatusCode.Failure)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result.Message);

                return Conflict(result.Message);
            }

            return Ok(result);
        }

        /// <summary>
        /// Deletes a property by its ID.
        /// </summary>
        /// <param name="propertyId">The unique identifier of the property to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("deleteProperty/{propertyId:guid}")]
        [ProducesResponseType(typeof(PropertyModelEntity), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProperty(Guid propertyId)
        {
            var result = await _propertyRepository.DeletePropertyAsync(propertyId);

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result);
        }
    }
}
