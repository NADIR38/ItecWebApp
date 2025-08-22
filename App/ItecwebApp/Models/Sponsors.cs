using System.ComponentModel.DataAnnotations;

namespace ItecwebApp.Models
{
    public class Sponsors
    {
        public int SponsorId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Sponsor name cannot exceed 100 characters.")]
        [Display(Name = "Sponsor Name")]

        public string? SponsorName { get; set; } 
        [Required]
        [Display(Name = "Contact No")]
        [StringLength(15, ErrorMessage = "Contact number cannot exceed 15 characters.")]
        [RegularExpression(@"^\+?[0-9]*$", ErrorMessage = "Contact number must be numeric and can start with a + sign.")]
        public string? ContactNo { get; set; }

    }
}
