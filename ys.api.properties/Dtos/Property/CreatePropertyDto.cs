namespace ys.api.properties.Dtos.Property;

/// <summary>
/// Represents the data transfer object (DTO) used to create a new property.
/// Contains the necessary details such as the Name, Description, and location coordinates of the property.
/// </summary>
public class CreatePropertyDto
{
    /// <summary>
    /// Gets or sets the Name of the property.
    /// This field is required and should not be empty.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Description of the property.
    /// Provides additional details or context about the property.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the latitude of the property location.
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude of the property location.
    /// </summary>
    public double? Longitude { get; set; }
}
