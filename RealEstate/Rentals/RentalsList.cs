using System.Collections.Generic;

namespace RealEstate.Rentals
{
    public class RentalsList
    {
        public IEnumerable<Rental> Rentals { get; set; }
        public RentalsFilter Filters { get; set; }
    }
}