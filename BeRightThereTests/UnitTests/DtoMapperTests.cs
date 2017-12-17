using System;
using System.Linq;
using BeRightThere.ClientModels;
using BeRightThere.Models;
using Xunit;

namespace BeRightThereTests.UnitTests
{
    public class DtoMapperTests
    {
        [Fact]
        public void CanConvertTripToDto()
        {
            var trip = new Trip
            {
                Id = 101,
                TripIdentifier = Guid.NewGuid().ToString(),
                Locations = TestUtils.GetDummyLocations()
            };

            var tripDto = DtoMapper.ConvertTripToDto(trip);

            Assert.Equal(trip.TripIdentifier, tripDto.TripIdentifier);
            TestUtils.AssertEqual(trip.Locations, tripDto.Locations);
        }

        [Fact]
        public void CanConvertLocationsToDto()
        {
            var locations = TestUtils.GetDummyLocations();

            var locationsDto = DtoMapper.ConvertLocationsToDto(locations);

            TestUtils.AssertEqual(locations, locationsDto.ToList());
        }

        [Fact]
        public void CanConvertLocationToDto()
        {
            var location = new Location
            {
                Latitude = 55.6739062,
                Longitude = 12.5556993
            };

            var locationDto = DtoMapper.ConvertLocationToDto(location);

            Assert.Equal(location.Latitude, locationDto.Latitude);
            Assert.Equal(location.Longitude, locationDto.Longitude);
        }

        [Fact]
        public void CanConvertDtoToLocation()
        {
            var locationDto = new LocationDto
            {
                Latitude = 55.6739062,
                Longitude = 12.5556993
            };

            var location = DtoMapper.ConvertDtoToLocation(locationDto);

            Assert.Equal(locationDto.Latitude, location.Latitude);
            Assert.Equal(locationDto.Longitude, location.Longitude);
        }
    }
}
