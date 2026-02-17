using NetTopologySuite.Geometries;
using ys.api.properties.Dtos.Property;
using ys.api.properties.Models;

namespace ys.api.properties.Mappers;

public static class PropertyMappers
{
    public static PropertyModelEntity ToPropertyFromCreatePropertyDto(this CreatePropertyDto propertyDto)
    {
        Point? location = null;
        if (propertyDto.Latitude.HasValue && propertyDto.Longitude.HasValue)
        {
            location = new Point(propertyDto.Longitude.Value, propertyDto.Latitude.Value) { SRID = 4326 };
        }

        return new PropertyModelEntity
        {
            id = Guid.NewGuid(),
            name = propertyDto.Name,
            description = propertyDto.Description,
            version = 1,
            created_on = DateTime.Now.ToUniversalTime(),
            updated_on = DateTime.Now.ToUniversalTime(),
            created_by = null,
            updated_by = null,
            level = 1,
            parent_id = null,
            meta_data = null,
            location = location,
            boundary = null,
        };
    }
}
