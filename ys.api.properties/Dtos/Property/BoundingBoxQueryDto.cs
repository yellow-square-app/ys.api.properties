namespace ys.api.properties.Dtos.Property;

/// <summary>
/// Represents the data transfer object for a bounding box spatial query.
/// Defines a rectangular geographic area using minimum and maximum latitude/longitude values.
/// </summary>
public class BoundingBoxQueryDto
{
    /// <summary>
    /// Gets or sets the minimum latitude (south boundary) of the bounding box.
    /// </summary>
    public double MinLat { get; set; }

    /// <summary>
    /// Gets or sets the minimum longitude (west boundary) of the bounding box.
    /// </summary>
    public double MinLng { get; set; }

    /// <summary>
    /// Gets or sets the maximum latitude (north boundary) of the bounding box.
    /// </summary>
    public double MaxLat { get; set; }

    /// <summary>
    /// Gets or sets the maximum longitude (east boundary) of the bounding box.
    /// </summary>
    public double MaxLng { get; set; }
}
