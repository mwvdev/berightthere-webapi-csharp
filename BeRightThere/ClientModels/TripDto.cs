using System.Collections.Generic;

namespace BeRightThere.ClientModels
{
    public class TripDto
    {
        private ICollection<LocationDto> _locations;

        public string TripIdentifier { get; set; }

        public ICollection<LocationDto> Locations
        {
            get => _locations ?? (_locations = new List<LocationDto>());
            set => _locations = value;
        }
    }
}