using ItecwebApp.Models;

namespace ItecwebApp.Interfaces
{
    public interface ICommiteeMembersDal
    {
        bool AddMembers(CommitteMemeber m);
        List<CommitteMemeber> getallmembers();
        List<CommitteMemeber> GetMembers(string commiteeName);
        List<CommitteMemeber> searchmembers(string searchTerm);
    }
}