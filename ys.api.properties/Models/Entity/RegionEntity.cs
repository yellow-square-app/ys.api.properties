using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ys.api.properties.Models;

/// <summary>
/// Represents an administrative region (state, province, territory, etc.)
/// from the ys-addresses-regions table.
/// </summary>
public class RegionEntity
{
    public Guid id { get; set; }

    [MaxLength(2)]
    [JsonPropertyName("countryCode")]
    public string country_code { get; set; } = string.Empty;

    [MaxLength(256)]
    public string name { get; set; } = string.Empty;

    [MaxLength(256)]
    [JsonPropertyName("displayName")]
    public string display_name { get; set; } = string.Empty;

    public string? description { get; set; }

    [MaxLength(100)]
    [JsonPropertyName("regionType")]
    public string region_type { get; set; } = string.Empty;

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
