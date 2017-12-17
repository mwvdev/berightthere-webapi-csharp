using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BeRightThere.ClientModels;
using Newtonsoft.Json;
using Xunit;

namespace BeRightThereTests.IntegrationTests
{
    public class TripControllerTests : IClassFixture<TestFixture<Startup>>
    {
        public TripControllerTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }

        private readonly HttpClient _client;
        private const string ApplicationJson = "application/json";

        [Theory]
        [InlineData(null, null)]
        [InlineData(55.6782377, null)]
        [InlineData(null, 12.5594759)]
        public async void AddLocation_WhenInvalidModel_ReturnsBadRequest(double? latitude, double? longitude)
        {
            var invalidDto = new
            {
                Latitude = latitude,
                Longitude = longitude
            };

            var tripDto = await Checkin();
            var response = await AddLocation(tripDto.TripIdentifier, SerializeContent(invalidDto));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseDto = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            Assert.Null(responseDto);
        }

        [Theory]
        [InlineData(91, 12.5594759)]
        [InlineData(55.6782377, 181)]
        [InlineData(91, 181)]
        [InlineData(-91, 12.5594759)]
        [InlineData(55.6782377, -181)]
        [InlineData(-91, -181)]
        public async void AddLocation_WhenInvalidLocations_ReturnsBadRequestWithObject(double latitude, double longitude)
        {
            var invalidDto = new LocationDto
            {
                Latitude = latitude,
                Longitude = longitude
            };

            var tripDto = await Checkin();
            var response = await AddLocation(tripDto.TripIdentifier, SerializeContent(invalidDto));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseDto = JsonConvert.DeserializeObject<LocationDto>(await response.Content.ReadAsStringAsync());
            Assert.Equal(invalidDto.Latitude, responseDto.Latitude);
            Assert.Equal(invalidDto.Longitude, responseDto.Longitude);
        }

        private StringContent SerializeContent(object content)
        {
            return new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, ApplicationJson);
        }

        [Fact]
        public async void AddLocation_WhenValidModel_ReturnsNoContentResult()
        {
            var locationDto = new LocationDto
            {
                Latitude = 55.6782377,
                Longitude = 12.5594759
            };

            var tripDto = JsonConvert.DeserializeObject<TripDto>(await _client.GetStringAsync("/api/trip/checkin"));
            var response = await AddLocation(tripDto.TripIdentifier, SerializeContent(locationDto));

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void GetLocations_ReturnsFilteredLocations_WhenValidOffsetRequest()
        {
            var locationDto1 = new LocationDto
            {
                Latitude = 55.6782377,
                Longitude = 12.5594759
            };
            var locationDto2 = new LocationDto
            {
                Latitude = 55.6782377,
                Longitude = 12.5594759
            };

            var tripDto = await Checkin();
            await AddLocation(tripDto.TripIdentifier, SerializeContent(locationDto1));
            await AddLocation(tripDto.TripIdentifier, SerializeContent(locationDto2));

            var response = await _client.GetAsync($"/api/trip/Locations/{tripDto.TripIdentifier}/1");
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseDto = JsonConvert.DeserializeObject<ICollection<LocationDto>>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(responseDto);
            Assert.Equal(1, responseDto.Count);

            var actualLocationDto = responseDto.First();
            Assert.Equal(locationDto2.Latitude, actualLocationDto.Latitude);
            Assert.Equal(locationDto2.Longitude, actualLocationDto.Longitude);
        }

        private async Task<TripDto> Checkin()
        {
            return JsonConvert.DeserializeObject<TripDto>(await _client.GetStringAsync("/api/trip/checkin"));
        }

        private async Task<HttpResponseMessage> AddLocation(string tripIdentifier, HttpContent locationContent)
        {
            return await _client.PostAsync($"/api/trip/addLocation/{tripIdentifier}", locationContent);
        }
    }
}