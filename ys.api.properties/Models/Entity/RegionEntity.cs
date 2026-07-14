using System.ComponentModel.DataAnnotations;

namespace ys.api.properties.Models;

/// <summary>
/// Represents an administrative region (state, province, territory, etc.)
/// from the ys-addresses-regions table.
/// </summary>
public class RegionEntity
{
    public Guid id { get; set; }

    [MaxLength(2)]
    public string country_code { get; set; } = string.Empty;

    [MaxLength(256)]
    public string name { get; set; } = string.Empty;

    [MaxLength(256)]
    public string display_name { get; set; } = string.Empty;

    public string? description { get; set; }

    [MaxLength(100)]
    public string region_type { get; set; } = string.Empty;

    public bool is_active { get; set; } = true;

    public string? meta_data { get; set; }

    public Guid? created_by { get; set; }

    public Guid? updated_by { get; set; }

    public DateTime? created_on { get; set; } = DateTime.Now;

    public DateTime? updated_on { get; set; }
}
