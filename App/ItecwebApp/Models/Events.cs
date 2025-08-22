using Google.Protobuf.WellKnownTypes;
using System.ComponentModel.DataAnnotations;

namespace ItecwebApp.Models
{
    public class Events
    {
        public int event_id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Event name cannot exceed 100 characters.")]
        public string event_name { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Event description cannot exceed 500 characters.")]
        public string event_description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime event_date { get; set; }
        [Required]
        public string venue_name { get; set; }
        [Required]
        public string committee_name { get; set; }
        [Required]
        [Range(2024, 2100, ErrorMessage = "Please select a valid edition.")]
        public int year { get; set; }
        [Required]
        public string category_name { get; set; }
      
        [Required(ErrorMessage = "Assigned time is required.")]
        [DataType(DataType.Time)]
        [Display(Name = "Assigned Time")]
        public DateTime assigned_time { get; set; }
        public Events()
        {
            // Default constructor
        }
        public Events(int eventId, string eventName, string eventDescription, DateTime eventDate, string venueName, string committeeName, int year, string categoryName, DateTime assigned_time)
        {
            event_id = eventId;
            event_name = eventName;
            event_description = eventDescription;
            event_date = eventDate;
            venue_name = venueName;
            committee_name = committeeName;
            this.year = year;
            category_name = categoryName;
            this.assigned_time = assigned_time;
        }

    }
}
