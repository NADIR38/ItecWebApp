using ItecwebApp.Models;

namespace ItecwebApp.Interfaces
{
    public interface IDutiesDAl
    {
        bool assign_duty(Duties d);
        List<Duties> getduties();
        List<Duties> search(string term);
        bool Updatestatus(Duties d);
    }
}