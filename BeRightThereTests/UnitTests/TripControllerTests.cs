using System;
using System.Collections.Generic;
using System.Linq;
using BeRightThere.ClientModels;
using BeRightThere.Controllers;
using BeRightThere.Interfaces;
using BeRightThere.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace BeRightThereTests.UnitTests
{
    public class TripControllerTests
    {
        [Theory]
        [InlineData(null, 0, 0)]
        [InlineData("", 0, 0)]
        [InlineData("invalid-guid", 0, 0)]
        [InlineData("zzzzzzzz-zzzz-zzzz-zzzz-zzzzzzzzzzzz", 0, 0)]
        public void AddLocation_ReturnsBadRequestObjectResult_WhenInvalidTripIdentifier(string tripIdentifier,
            double latitude, double longitude)
        {
            var trip = new Trip
            {
                TripIdentifier = tripIdentifier
            };

            var mockRepository = Substitute.For<ITripRepository>();
            mockRepository.FindTrip(tripIdentifier).Returns(trip);
            var controller = new TripController(mockRepository);

            var result = controller.AddLocation(trip.TripIdentifier, new LocationDto
            {
                Latitude = latitude,
                Longitude = longitude
            });
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("ab7d8df0-e952-4956-8c38-0351b90fd045")]
        public void GetLocations_ReturnsNotFoundResult_WhenUnknownTripIdentifier(string tripIdentifier)
        {
            var mockRepository = Substitute.For<ITripRepository>();
            mockRepository.FindTrip(tripIdentifier).Returns(x => null);
            var controller = new TripController(mockRepository);

            var result = controller.GetLocations(tripIdentifier);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void AddLocation_ReturnsBadRequestObjectResult_WhenModelStateIsInvalid()
        {
            var mockRepository = Substitute.For<ITripRepository>();
            var controller = new TripController(mockRepository);
            controller.ModelState.AddModelError("Latitude", "Value out of range");
            controller.ModelState.AddModelError("Longitude", "Value out of range");
            var location = new LocationDto
            {
                Latitude = -91,
                Longitude = 181
            };

            var result = controller.AddLocation("ab7d8df0-e952-4956-8c38-0351b90fd045", location);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var badRequestLocation = Assert.IsType<LocationDto>(badRequestResult.Value);
            Assert.Equal(location.Latitude, badRequestLocation.Latitude);
            Assert.Equal(location.Longitude, badRequestLocation.Longitude);
        }

        [Fact]
        public void AddLocation_ReturnsNoContentResult_WhenValidRequest()
        {
            const string tripIdentifier = "ab7d8df0-e952-4956-8c38-0351b90fd045";
            var trip = new Trip
            {
                TripIdentifier = tripIdentifier
            };

            var mockRepository = Substitute.For<ITripRepository>();
            mockRepository.FindTrip(tripIdentifier).Returns(trip);
            var controller = new TripController(mockRepository);

            var result = controller.AddLocation(tripIdentifier, new LocationDto
            {
                Latitude = 55.6782377,
                Longitude = 12.5594759
            });

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void AddLocation_ReturnsNotFoundResult_WhenUnknownTripIdentifier()
        {
            const string tripIdentifier = "ab7d8df0-e952-4956-8c38-0351b90fd045";

            var mockRepository = Substitute.For<ITripRepository>();
            mockRepository.FindTrip(tripIdentifier).Returns(x => null);
            var controller = new TripController(mockRepository);

            var result = controller.AddLocation(tripIdentifier, new LocationDto
            {
                Latitude = 55.6782377,
                Longitude = 12.5594759
            });

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Checkin_ShouldCreateGuid()
        {
            var mockRepository = Substitute.For<ITripRepository>();
            var controller = new TripController(mockRepository);

            var result = controller.Checkin();
            var tripResult = Assert.IsType<TripDto>(result);
            Assert.True(Guid.TryParse(tripResult.TripIdentifier, out var _));
        }

        [Fact]
        public void GetLocations_ReturnsLocations_WhenValidRequest()
        {
            const string tripIdentifier = "ab7d8df0-e952-4956-8c38-0351b90fd045";
            var trip = new Trip
            {
                TripIdentifier = tripIdentifier,
                Locations = TestUtils.GetDummyLocations()
            };

            var mockRepository = Substitute.For<ITripRepository>();
            mockRepository.FindTrip(tripIdentifier).Returns(trip);

            var controller = new TripController(mockRepository);

            var result = controller.GetLocations(tripIdentifier);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseLocations = Assert.IsAssignableFrom<IEnumerable<LocationDto>>(okResult.Value);

            TestUtils.AssertEqual(trip.Locations, responseLocations.ToList());
        }
    }
}