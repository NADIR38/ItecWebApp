using ItecwebApp.Models;

namespace ItecwebApp.Interfaces
{
    public interface IVendorsDAl
    {
        bool AddVendor(Vendors v);
        bool EditVendor(Vendors v);
        List<Vendors> GetVendors();
        List<Vendors> Search(string term);
    }
}