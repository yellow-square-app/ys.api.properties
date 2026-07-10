namespace ys.api.properties.Dtos.Address;

/// <summary>
/// Represents the data transfer object (DTO) used to create a new address.
/// Uses snake_case property names to match the entity and frontend conventions.
/// </summary>
public class CreateAddressDto
{
    public string? name { get; set; }
    public string? description { get; set; }
    public string address_line_1 { get; set; } = string.Empty;
    public string? address_line_2 { get; set; }
    public string? address_line_3 { get; set; }
    public string? address_line_4 { get; set; }
    public string? locality { get; set; }
    public string? region_name { get; set; }
    public string? region_type { get; set; }
    public string? sub_region_name { get; set; }
    public string? sub_region_type { get; set; }
    public string? postal_code { get; set; }
    public string? postal_code_type { get; set; }
    public string? country_code { get; set; }
    public Guid? parent_id { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
}
