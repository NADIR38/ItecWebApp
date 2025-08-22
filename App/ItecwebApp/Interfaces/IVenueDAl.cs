using ItecwebApp.Models;

namespace ItecwebApp.Interfaces
{
    public interface IVenueDAl
    {
        bool addvenue(Venues v);
        List<Venues> GetVenues();
        List<string> GetVenueNames(string name);
        bool EditVenue(Venues v);
        List<Venues> searchvenues(string term);
    }
}