namespace ys.api.properties.Dtos.Property;

/// <summary>
/// Represents the data transfer object for an overlap/intersection spatial query.
/// Defines a polygon using a list of coordinate pairs [longitude, latitude].
/// </summary>
public class OverlapQueryDto
{
    /// <summary>
    /// Gets or sets the list of coordinate pairs defining the polygon.
    /// Each inner array should contain exactly two elements: [longitude, latitude].
    /// At least 3 coordinate pairs are required to form a valid polygon.
    /// </summary>
    public List<double[]> Coordinates { get; set; } = new();
}
