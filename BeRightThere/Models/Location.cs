using System.ComponentModel.DataAnnotations;

namespace BeRightThere.Models
{
    public class Location
    {
        public int Id { get; set; }
        public int TripId { get; set; }

        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        public double Longitude { get; set; }

        public Trip Trip { get; set; }
    }
}