using System.Collections.Generic;
using BeRightThere.Models;

namespace BeRightThere.Interfaces
{
    public interface ITripRepository
    {
        void Add(Trip trip);
        void Update(Trip trip);
        Trip FindTrip(string tripIdentifier);
        IEnumerable<Trip> ListAll();
    }
}