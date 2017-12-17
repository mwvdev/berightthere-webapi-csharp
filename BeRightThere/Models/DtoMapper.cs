using System.Collections.Generic;
using System.Linq;
using BeRightThere.ClientModels;

namespace BeRightThere.Models
{
    public static class DtoMapper
    {
        public static TripDto ConvertTripToDto(Trip trip)
        {
            return new TripDto
            {
                TripIdentifier = trip.TripIdentifier,
                Locations = ConvertLocationsToDto(trip.Locations).ToList()
            };
        }

        public static IEnumerable<LocationDto> ConvertLocationsToDto(IEnumerable<Location> locations)
        {
            return locations.Select(ConvertLocationToDto);
        }

        public static LocationDto ConvertLocationToDto(Location location)
        {
            return new LocationDto
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }

        public static Location ConvertDtoToLocation(LocationDto locationDto)
        {
            return new Location
            {
                Latitude = locationDto.Latitude,
                Longitude = locationDto.Longitude
            };
        }
    }
}