using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ys.api.properties.Dtos.Address;

/// <summary>
/// Represents the data transfer object (DTO) used to create a new address.
/// Accepts camelCase JSON keys from the client.
/// </summary>
public class CreateAddressDto
{
    public string? name { get; set; }
    public string? description { get; set; }

    [Required(ErrorMessage = "addressLine1 is required.")]
    [MaxLength(256, ErrorMessage = "addressLine1 must not exceed 256 characters.")]
    [JsonPropertyName("addressLine1")]
    public string address_line_1 { get; set; } = string.Empty;

    [MaxLength(256)]
    [JsonPropertyName("addressLine2")]
    public string? address_line_2 { get; set; }

    [MaxLength(256)]
    [JsonPropertyName("addressLine3")]
    public string? address_line_3 { get; set; }

    [MaxLength(256)]
    [JsonPropertyName("addressLine4")]
    public string? address_line_4 { get; set; }

    [MaxLength(256)]
    public string? city { get; set; }

    [MaxLength(256)]
    public string? state { get; set; }

    [JsonPropertyName("regionType")]
    public string? region_type { get; set; }

    [JsonPropertyName("subRegionName")]
    public string? sub_region_name { get; set; }

    [JsonPropertyName("subRegionType")]
    public string? sub_region_type { get; set; }

    [MaxLength(20)]
    [JsonPropertyName("postalCode")]
    public string? postal_code { get; set; }

    [JsonPropertyName("postalCodeType")]
    public string? postal_code_type { get; set; }

    [Required(ErrorMessage = "countryCode is required.")]
    [MaxLength(10, ErrorMessage = "countryCode must not exceed 10 characters.")]
    [JsonPropertyName("countryCode")]
    public string country_code { get; set; } = string.Empty;

    [JsonPropertyName("isAddressVerified")]
    public bool is_address_verified { get; set; }

    [Required(ErrorMessage = "parentId is required.")]
    [JsonPropertyName("parentId")]
    public Guid? parent_id { get; set; }

    [Required(ErrorMessage = "createdBy is required.")]
    [JsonPropertyName("createdBy")]
    public Guid? created_by { get; set; }

    public double? latitude { get; set; }
    public double? longitude { get; set; }
}
