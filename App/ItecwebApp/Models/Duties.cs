using System.ComponentModel.DataAnnotations;

namespace ItecwebApp.Models
{
    public class Duties
    {
        public int duty_id { get; set; }
    public string committee_name { get; set; }
        public string name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MinLength(3, ErrorMessage = "Description must be at least 3 characters long")]
        [MaxLength(100, ErrorMessage = "Description cannot be longer than 100 characters")]
        [Display(Name = "Description")]
        public string description { get; set; }
        [Required(ErrorMessage = "Deadline is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; }
        public string status { get; set; }
    }
}
