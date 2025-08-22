using System.ComponentModel.DataAnnotations;

namespace ItecwebApp.Models
{
    public class Venues
    {
        public int id { get; set; }
        [Required(ErrorMessage ="Venue name is required")]
        [StringLength(100, ErrorMessage = "Venue name cannot exceed 100 characters")]
        [Display(Name = "Venue Name")]

        public string name { get; set; }
        [Required(ErrorMessage = "Venue address is required")]
        [StringLength(200, ErrorMessage = "Venue address cannot exceed 200 characters")]
        [Display(Name = "Venue Address")]
        public string location { get; set; }
        [Required(ErrorMessage = "Venue capacity is required")]
        [Range(1, 100000, ErrorMessage = "Venue capacity must be between 1 and 100,000")]
        [Display(Name = "Venue Capacity")]
        public int capacity { get; set; }
        public Venues() { }
        public Venues(int id, string name, string location, int capacity)
        {
            this.id = id;
            this.name = name;
            this.location = location;
            this.capacity = capacity;
        }
    }
}
