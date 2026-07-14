using System.ComponentModel.DataAnnotations;

namespace ys.api.properties.Models;

/// <summary>
/// Represents a country record from the ys-addresses-country-data table.
/// Contains ISO 3166-1 data for international address handling.
/// </summary>
public class CountryDataEntity
{
    public Guid id { get; set; }

    [MaxLength(256)]
    public string name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? sovereignty { get; set; }

    [MaxLength(2)]
    public string alpha_2_code { get; set; } = string.Empty;

    [MaxLength(3)]
    public string alpha_3_code { get; set; } = string.Empty;

    [MaxLength(3)]
    public string numeric_code { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? subdivision_codes_link { get; set; }

    [MaxLength(50)]
    public string? tld { get; set; }

    public string? description { get; set; }

    public bool is_active { get; set; } = true;

    public string? meta_data { get; set; }

    public Guid? created_by { get; set; }

    public Guid? updated_by { get; set; }

    public DateTime? created_on { get; set; } = DateTime.Now;

    public DateTime? updated_on { get; set; }
}
