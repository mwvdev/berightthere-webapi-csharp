using System;
using System.Collections.Generic;
using System.Linq;
using BeRightThere.ClientModels;
using BeRightThere.Models;
using Xunit;

namespace BeRightThereTests.UnitTests
{
    public static class TestUtils
    {
        public static List<Location> GetDummyLocations()
        {
            return new List<Location>
            {
                new Location
                {
                    Latitude = 55.6739062,
                    Longitude = 12.5556993
                },
                new Location
                {
                    Latitude = 55.6746322,
                    Longitude = 12.5585318
                },
                new Location
                {
                    Latitude = 55.6764229,
                    Longitude = 12.5588751
                }
            };
        }

        public static void AssertEqual(ICollection<Location> expectedLocations, ICollection<LocationDto> locationsDto)
        {
            var expectedLocationsDto = expectedLocations.Select(location => new LocationDto
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            }).ToList();

            Assert.Equal(expectedLocationsDto, locationsDto, new LocationDtoComparer());
        }

        internal class LocationDtoComparer : IEqualityComparer<LocationDto>
        {
            private const double Tolerance = 0.0001;

            public bool Equals(LocationDto x, LocationDto y)
            {
                return Math.Abs(x.Latitude - y.Latitude) < Tolerance && Math.Abs(x.Longitude - y.Longitude) < Tolerance;
            }

            public int GetHashCode(LocationDto locationDto)
            {
                throw new NotImplementedException();
            }
        }
    }
}