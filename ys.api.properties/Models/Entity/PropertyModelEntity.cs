using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

namespace ys.api.properties.Models
{
    /// <summary>
    /// Represents the entity model for Property in the database.
    /// Uses NetTopologySuite geometry types for PostGIS spatial columns.
    /// </summary>
    public class PropertyModelEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the property entity.
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// Gets or sets the name of the property entity.
        /// The length is restricted to a maximum of 100 characters.
        /// </summary>
        [MaxLength(100)]
        public string name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a brief description of the property entity.
        /// The length is restricted to a maximum of 100 characters.
        /// </summary>
        [MaxLength(100)]
        public string description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version number of the property entity.
        /// </summary>
        public int version { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the property entity was created.
        /// Defaults to the current UTC time if not explicitly provided.
        /// </summary>
        [JsonPropertyName("createdOn")]
        public DateTime? created_on { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the UTC timestamp when the property entity was last updated.
        /// </summary>
        [JsonPropertyName("updatedOn")]
        public DateTime? updated_on { get; set; }

        /// <summary>
        /// Gets or sets the user_id for the user who created the entity.
        /// </summary>
        [JsonPropertyName("createdBy")]
        public Guid? created_by { get; set; }

        /// <summary>
        /// Gets or sets the user_id for the user who edited the entity.
        /// </summary>
        [JsonPropertyName("updatedBy")]
        public Guid? updated_by { get; set; }

        /// <summary>
        /// Gets or sets the level associated with the property entity.
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// Gets or sets the nullable parent_id for the property.
        /// </summary>
        [JsonPropertyName("parentId")]
        public Guid? parent_id { get; set; }

        /// <summary>
        /// Gets or sets the first line of the address.
        /// </summary>
        [MaxLength(256)]
        [JsonPropertyName("addressLine1")]
        public string? address_line_1 { get; set; }

        /// <summary>
        /// Gets or sets the second line of the address.
        /// </summary>
        [MaxLength(256)]
        [JsonPropertyName("addressLine2")]
        public string? address_line_2 { get; set; }

        /// <summary>
        /// Gets or sets the third line of the address.
        /// </summary>
        [MaxLength(256)]
        [JsonPropertyName("addressLine3")]
        public string? address_line_3 { get; set; }

        /// <summary>
        /// Gets or sets the fourth line of the address.
        /// </summary>
        [MaxLength(256)]
        [JsonPropertyName("addressLine4")]
        public string? address_line_4 { get; set; }

        /// <summary>
        /// Gets or sets the city of the address.
        /// </summary>
        [MaxLength(256)]
        public string? city { get; set; }

        /// <summary>
        /// Gets or sets the state of the address.
        /// </summary>
        [MaxLength(256)]
        public string? state { get; set; }

        /// <summary>
        /// Gets or sets the region type (e.g., state, province, prefecture).
        /// </summary>
        [MaxLength(100)]
        [JsonPropertyName("regionType")]
        public string? region_type { get; set; }

        /// <summary>
        /// Gets or sets the sub-region name (county/district).
        /// </summary>
        [MaxLength(256)]
        [JsonPropertyName("subRegionName")]
        public string? sub_region_name { get; set; }

        /// <summary>
        /// Gets or sets the sub-region type.
        /// </summary>
        [MaxLength(100)]
        [JsonPropertyName("subRegionType")]
        public string? sub_region_type { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        [MaxLength(20)]
        [JsonPropertyName("postalCode")]
        public string? postal_code { get; set; }

        /// <summary>
        /// Gets or sets the postal code type (e.g., ZIP, postcode).
        /// </summary>
        [MaxLength(50)]
        [JsonPropertyName("postalCodeType")]
        public string? postal_code_type { get; set; }

        /// <summary>
        /// Gets or sets the country code (e.g., US, GB, DE).
        /// </summary>
        [MaxLength(10)]
        [JsonPropertyName("countryCode")]
        public string? country_code { get; set; }

        /// <summary>
        /// Gets or sets whether the address has been verified by a third-party service.
        /// </summary>
        [JsonPropertyName("isAddressVerified")]
        public bool is_address_verified { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of the address.
        /// </summary>
        public double? latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude coordinate of the address.
        /// </summary>
        public double? longitude { get; set; }

        /// <summary>
        /// Gets or sets metadata for the property entity as a JSON string.
        /// </summary>
        [JsonPropertyName("metaData")]
        public string? meta_data { get; set; }

        /// <summary>
        /// Gets or sets the geographic point location (SRID 4326) for the property.
        /// Maps to a PostGIS geometry(Point, 4326) column.
        /// </summary>
        public Point? location { get; set; }

        /// <summary>
        /// Gets or sets the geographic polygon boundary (SRID 4326) for the property.
        /// Maps to a PostGIS geometry(Polygon, 4326) column.
        /// </summary>
        public Polygon? boundary { get; set; }
    }
}
