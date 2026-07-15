using System.ComponentModel.DataAnnotations;

namespace ys.api.properties.Dtos.Address;

/// <summary>
/// Represents the data transfer object (DTO) used to update an existing address.
/// Uses snake_case property names to match the entity and frontend conventions.
/// </summary>
public class UpdateAddressDto
{
    [Required(ErrorMessage = "address_id is required.")]
    public Guid address_id { get; set; }

    public string? name { get; set; }
    public string? description { get; set; }

    [Required(ErrorMessage = "address_line_1 is required.")]
    [MaxLength(256, ErrorMessage = "address_line_1 must not exceed 256 characters.")]
    public string address_line_1 { get; set; } = string.Empty;

    [MaxLength(256)]
    public string? address_line_2 { get; set; }

    [MaxLength(256)]
    public string? address_line_3 { get; set; }

    [MaxLength(256)]
    public string? address_line_4 { get; set; }

    [MaxLength(256)]
    public string? city { get; set; }

    [MaxLength(256)]
    public string? state { get; set; }

    public string? region_type { get; set; }
    public string? sub_region_name { get; set; }
    public string? sub_region_type { get; set; }

    [MaxLength(20)]
    public string? postal_code { get; set; }

    public string? postal_code_type { get; set; }

    [Required(ErrorMessage = "country_code is required.")]
    [MaxLength(10, ErrorMessage = "country_code must not exceed 10 characters.")]
    public string country_code { get; set; } = string.Empty;

    public bool is_address_verified { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
}
