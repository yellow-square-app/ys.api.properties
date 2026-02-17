using System.ComponentModel.DataAnnotations;

namespace ys.api.properties.Models.Domain
{
    /// <summary>
    /// Represents a property model within the domain. Provides general properties for properties,
    /// such as Name, Description, versioning, location coordinates, and timestamps for creation and updates.
    /// Uses double Latitude/Longitude instead of NTS types for serialization.
    /// </summary>
    public class PropertyModelDomain
    {
        /// <summary>
        /// Gets or sets the unique identifier for the property model.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the Name of the property model.
        /// The length is restricted to a maximum of 100 characters.
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a brief Description of the property model.
        /// The length is restricted to a maximum of 100 characters.
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version number of the property model.
        /// This is used to represent the iteration or version of this entity.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the property model was created.
        /// Defaults to the current UTC time if not explicitly provided.
        /// </summary>
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the UTC timestamp when the property model was last updated.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the user_id for the user who created the model.
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the user_id for the user who edited the model.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the level associated with the property model.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the nullable parent_id for the property.
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Gets or sets metadata for the property model as a JSON string.
        /// </summary>
        public string? MetaData { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the property location.
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the property location.
        /// </summary>
        public double? Longitude { get; set; }
    }
}
