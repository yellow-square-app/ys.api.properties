using System.ComponentModel.DataAnnotations;

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
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100, ErrorMessage = "Name must not exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Description of the property.
    /// Provides additional details or context about the property.
    /// </summary>
    [MaxLength(100, ErrorMessage = "Description must not exceed 100 characters.")]
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
