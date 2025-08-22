using ItecwebApp.Models;

namespace ItecwebApp.Interfaces
{
    public interface IParticipantsDAL
    {
        bool AddParticipant(Participants participant);
        bool DeleteParticipant(int id);
        List<Participants> GetAllParticipants();
        List<Participants> SearchParticipants(string searchTerm);
        bool UpdateParticipant(Participants participant);
    }
}