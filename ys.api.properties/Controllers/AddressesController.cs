using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ys.api.properties.Constants;
using ys.api.properties.Data;
using ys.api.properties.Dtos.Address;
using ys.api.properties.Interfaces;
using ys.api.properties.Helpers;
using ys.api.properties.Mappers;
using ys.api.properties.Models;

namespace ys.api.properties.Controllers
{
    /// <summary>
    /// Controller for managing addresses.
    /// </summary>
    [Route("api/addresses")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly ILogger<AddressesController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IAddressRepository _addressRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressesController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="context">The database context.</param>
        /// <param name="addressRepository">The address repository.</param>
        public AddressesController(ILogger<AddressesController> logger, ApplicationDbContext context,
            IAddressRepository addressRepository)
        {
            _logger = logger;
            _context = context;
            _addressRepository = addressRepository;
        }

        /// <summary>
        /// Retrieves all addresses.
        /// </summary>
        /// <returns>A list of addresses.</returns>
        [HttpGet("get-addresses")]
        [ProducesResponseType(typeof(IEnumerable<PropertyModelEntity>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAddresses()
        {
            var result = await _addressRepository.GetAddressesAsync();

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result?.Data);
        }

        /// <summary>
        /// Retrieves a specific address by its ID.
        /// </summary>
        /// <param name="addressId">The unique identifier of the address.</param>
        /// <returns>The requested address.</returns>
        [HttpGet("get-address-by-id/{addressId:guid}")]
        [ProducesResponseType(typeof(PropertyModelEntity), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAddressById(Guid addressId)
        {
            var result = await _addressRepository.GetAddressByIdAsync(addressId);

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result?.Data);
        }

        /// <summary>
        /// Retrieves all addresses associated with a given parent ID.
        /// </summary>
        /// <param name="parentId">The parent identifier to filter by.</param>
        /// <returns>A list of addresses for the given parent.</returns>
        [HttpGet("get-addresses-by-parent-id/{parentId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<PropertyModelEntity>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAddressesByParentId(Guid parentId)
        {
            var result = await _addressRepository.GetAddressesByParentIdAsync(parentId);

            if (result?.Status == Models.Results.StatusCode.Failure)
                return NotFound(result.Message);

            return Ok(result?.Data);
        }

        /// <summary>
        /// Creates a new address.
        /// </summary>
        /// <param name="addressDto">The address data to create.</param>
        /// <returns>The created address.</returns>
        [HttpPost("create-address")]
        [ProducesResponseType(typeof(PropertyModelEntity), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDto addressDto)
        {
            try
            {
                var result = await _addressRepository.CreateAddressAsync(addressDto.ToPropertyFromCreateAddressDto());

                if (result?.Status == Models.Results.StatusCode.Failure)
                    return BadRequest(Messages.Errors.ErrorCreatingValue("address"));

                return CreatedAtAction(nameof(GetAddressById), new { addressId = result?.Data?.id }, result);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating address");
                var detail = DbExceptionHelper.ExtractDetail(ex);
                return BadRequest(new { error = "Failed to create address.", detail });
            }
        }

        /// <summary>
        /// Updates an existing address.
        /// </summary>
        /// <param name="addressId">The unique identifier of the address to update.</param>
        /// <param name="addressDto">The updated address data.</param>
        /// <returns>The updated address.</returns>
        [HttpPut("update-address/{addressId:guid}")]
        [ProducesResponseType(typeof(PropertyModelEntity), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateAddress(Guid addressId, [FromBody] UpdateAddressDto addressDto)
        {
            if (addressId != addressDto.address_id)
                return BadRequest("Address ID mismatch");

            try
            {
                var result = await _addressRepository.UpdateAddressAsync(addressDto);

                if (result?.Status == Models.Results.StatusCode.Failure)
                    return NotFound(result.Message);

                return Ok(result);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating address {AddressId}", addressId);
                var detail = DbExceptionHelper.ExtractDetail(ex);
                return BadRequest(new { error = "Failed to update address.", detail });
            }
        }

        /// <summary>
        /// Deletes an address by its ID.
        /// </summary>
        /// <param name="addressId">The unique identifier of the address to delete.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("delete-address/{addressId:guid}")]
        [ProducesResponseType(typeof(PropertyModelEntity), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            try
            {
                var result = await _addressRepository.DeleteAddressAsync(addressId);

                if (result?.Status == Models.Results.StatusCode.Failure)
                    return NotFound(result.Message);

                return Ok(result);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while deleting address {AddressId}", addressId);
                var detail = DbExceptionHelper.ExtractDetail(ex);
                return BadRequest(new { error = "Failed to delete address.", detail });
            }
        }
    }
}
