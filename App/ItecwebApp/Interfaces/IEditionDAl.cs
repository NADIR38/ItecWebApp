using ItecwebApp.Models;

namespace ItecwebApp.Interfaces
{
    public interface IEditionDAl
    {
        bool AddEdition(Edition e);
        List<Edition> GetEditions();
    }
}