using ItecwebApp.Models;

namespace ItecwebApp.Interfaces
{
    public interface ISponsorsDAl
    {
        bool AddSponsor(Sponsors sponsor);
        List<Sponsors> GetSponsors();
        List<Sponsors> searchsponsors(string searchTerm);
        bool UpdateSponsor(Sponsors sponsor);
    }
}