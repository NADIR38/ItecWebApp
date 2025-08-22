using System.ComponentModel.DataAnnotations;

namespace ItecwebApp.Models
{
    public class Vendors
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vendor name is required.")]
        [StringLength(100, ErrorMessage = "Vendor name cannot exceed 100 characters.")]
        [Display(Name = "Vendor Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Contact person is required.")]
        [StringLength(100, ErrorMessage = "Contact person cannot exceed 100 characters.")]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        [Required(ErrorMessage = "Service_Type is required.")]
        [StringLength(50, ErrorMessage = "Service type cannot exceed 50 characters.")]
        [Display(Name = "Service Type")]
        public string ServiceType { get; set; }
    }
}
