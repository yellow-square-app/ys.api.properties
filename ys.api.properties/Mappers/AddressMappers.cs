using NetTopologySuite.Geometries;
using ys.api.properties.Dtos.Address;
using ys.api.properties.Models;

namespace ys.api.properties.Mappers;

public static class AddressMappers
{
    public static PropertyModelEntity ToPropertyFromCreateAddressDto(this CreateAddressDto addressDto)
    {
        Point? location = null;
        if (addressDto.latitude.HasValue && addressDto.longitude.HasValue)
        {
            location = new Point(addressDto.longitude.Value, addressDto.latitude.Value) { SRID = 4326 };
        }

        return new PropertyModelEntity
        {
            id = Guid.NewGuid(),
            parent_id = addressDto.parent_id,
            name = addressDto.name ?? string.Empty,
            description = addressDto.description ?? string.Empty,
            address_line_1 = addressDto.address_line_1,
            address_line_2 = addressDto.address_line_2,
            address_line_3 = addressDto.address_line_3,
            address_line_4 = addressDto.address_line_4,
            city = addressDto.city,
            state = addressDto.state,
            region_type = addressDto.region_type,
            sub_region_name = addressDto.sub_region_name,
            sub_region_type = addressDto.sub_region_type,
            postal_code = addressDto.postal_code,
            postal_code_type = addressDto.postal_code_type,
            country_code = addressDto.country_code,
            is_address_verified = addressDto.is_address_verified,
            latitude = addressDto.latitude,
            longitude = addressDto.longitude,
            version = 1,
            level = 1,
            created_on = DateTime.Now.ToUniversalTime(),
            updated_on = DateTime.Now.ToUniversalTime(),
            created_by = addressDto.created_by,
            updated_by = null,
            meta_data = null,
            location = location,
            boundary = null,
        };
    }
}
