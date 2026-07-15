using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    [JsonPropertyName("alpha2Code")]
    public string alpha_2_code { get; set; } = string.Empty;

    [MaxLength(3)]
    [JsonPropertyName("alpha3Code")]
    public string alpha_3_code { get; set; } = string.Empty;

    [MaxLength(3)]
    [JsonPropertyName("numericCode")]
    public string numeric_code { get; set; } = string.Empty;

    [MaxLength(20)]
    [JsonPropertyName("subdivisionCodesLink")]
    public string? subdivision_codes_link { get; set; }

    [MaxLength(50)]
    public string? tld { get; set; }

    public string? description { get; set; }

    [JsonPropertyName("isActive")]
    public bool is_active { get; set; } = true;

    [JsonPropertyName("metaData")]
    public string? meta_data { get; set; }

    [JsonPropertyName("createdBy")]
    public Guid? created_by { get; set; }

    [JsonPropertyName("updatedBy")]
    public Guid? updated_by { get; set; }

    [JsonPropertyName("createdOn")]
    public DateTime? created_on { get; set; } = DateTime.Now;

    [JsonPropertyName("updatedOn")]
    public DateTime? updated_on { get; set; }
}
