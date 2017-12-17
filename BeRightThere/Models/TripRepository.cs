using System;
using System.Collections.Generic;
using System.Linq;
using BeRightThere.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeRightThere.Models
{
    public class TripRepository : ITripRepository
    {
        private readonly TripDbContext _dbContext;

        public TripRepository(TripDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Trip trip)
        {
            _dbContext.Add(trip);
            _dbContext.SaveChanges();
        }

        public void Update(Trip trip)
        {
            _dbContext.Update(trip);
            _dbContext.SaveChanges();
        }

        public Trip FindTrip(string tripIdentifier)
        {
            return _dbContext.Trips
                .Where(t => t.TripIdentifier == tripIdentifier)
                .Include(t => t.Locations)
                .SingleOrDefault();
        }

        public IEnumerable<Trip> ListAll()
        {
            return _dbContext.Trips.AsEnumerable();
        }
    }
}