using System.Collections.Generic;

namespace BeRightThere.Models
{
    public class Trip
    {
        private ICollection<Location> _locations;

        public int Id { get; set; }

        public string TripIdentifier { get; set; }

        public ICollection<Location> Locations
        {
            get => _locations ?? (_locations = new List<Location>());
            set => _locations = value;
        }
    }
}