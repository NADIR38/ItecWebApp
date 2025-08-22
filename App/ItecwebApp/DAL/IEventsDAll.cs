using ItecwebApp.Models;

namespace ItecwebApp.DAL
{
    public interface IEventsDAll
    {
        bool AddEvent(Events e);
        List<Events> GetEvents(string searchTerm = "");
        bool updatevent(Events e);
    }
}