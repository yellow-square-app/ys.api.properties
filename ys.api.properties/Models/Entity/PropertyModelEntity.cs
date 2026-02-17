using System.ComponentModel.DataAnnotations;
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
        public DateTime? created_on { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the UTC timestamp when the property entity was last updated.
        /// </summary>
        public DateTime? updated_on { get; set; }

        /// <summary>
        /// Gets or sets the user_id for the user who created the entity.
        /// </summary>
        public Guid? created_by { get; set; }

        /// <summary>
        /// Gets or sets the user_id for the user who edited the entity.
        /// </summary>
        public Guid? updated_by { get; set; }

        /// <summary>
        /// Gets or sets the level associated with the property entity.
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// Gets or sets the nullable parent_id for the property.
        /// </summary>
        public Guid? parent_id { get; set; }

        /// <summary>
        /// Gets or sets metadata for the property entity as a JSON string.
        /// </summary>
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
