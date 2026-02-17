namespace ys.api.properties.Dtos.Property;

/// <summary>
/// Represents the data transfer object for a radius-based spatial query.
/// Defines a circular geographic area using a center point and radius in meters.
/// </summary>
public class RadiusQueryDto
{
    /// <summary>
    /// Gets or sets the latitude of the center point.
    /// </summary>
    public double Lat { get; set; }

    /// <summary>
    /// Gets or sets the longitude of the center point.
    /// </summary>
    public double Lng { get; set; }

    /// <summary>
    /// Gets or sets the search radius in meters.
    /// </summary>
    public double RadiusMeters { get; set; }
}
