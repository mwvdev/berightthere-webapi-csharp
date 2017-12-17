using System;
using System.Linq;
using BeRightThere.ClientModels;
using BeRightThere.Interfaces;
using BeRightThere.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeRightThere.Controllers
{
    [Route("api/[controller]")]
    public class TripController : Controller
    {
        private readonly ITripRepository _tripRepository;

        public TripController(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        [HttpGet("Checkin")]
        public TripDto Checkin()
        {
            var trip = new Trip
            {
                TripIdentifier = Guid.NewGuid().ToString()
            };
            _tripRepository.Add(trip);

            return DtoMapper.ConvertTripToDto(trip);
        }

        [HttpPost("AddLocation/{tripIdentifier}")]
        public IActionResult AddLocation(string tripIdentifier, [FromBody] LocationDto locationDto)
        {
            if (!IsValidTripIdentifier(tripIdentifier))
                return BadRequest(tripIdentifier);
            if (!ModelState.IsValid)
                return BadRequest(locationDto);

            var trip = _tripRepository.FindTrip(tripIdentifier);
            if (trip == null)
                return NotFound();

            trip.Locations.Add(DtoMapper.ConvertDtoToLocation(locationDto));
            _tripRepository.Update(trip);

            return new NoContentResult();
        }

        [HttpGet("Locations/{tripIdentifier}/{fromIndex?}")]
        public IActionResult GetLocations(string tripIdentifier, int fromIndex = 0)
        {
            if (string.IsNullOrEmpty(tripIdentifier))
                return NotFound();
            var trip = _tripRepository.FindTrip(tripIdentifier);
            if (trip == null)
                return NotFound();

            var locations = trip.Locations.Where((location, index) => index >= fromIndex);
            return Ok(DtoMapper.ConvertLocationsToDto(locations));
        }

        private bool IsValidTripIdentifier(string tripIdentifier)
        {
            if (string.IsNullOrEmpty(tripIdentifier))
                return false;

            return Guid.TryParse(tripIdentifier, out var _);
        }
    }
}