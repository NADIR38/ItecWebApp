using System.ComponentModel.DataAnnotations;

namespace ItecwebApp.Models
{
    public class Edition
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(2000, 2100, ErrorMessage = "Year must be between 2000 and 2100")]
        public int Year { get;  set; }

        [Required(ErrorMessage = "Theme is required")]
        [MinLength(3, ErrorMessage = "Theme must be at least 3 characters long")]
        [MaxLength(100, ErrorMessage = "Theme cannot be longer than 100 characters")]
        public string Theme { get; set; }
        [Required(ErrorMessage = "description is required")]
        [MinLength(3, ErrorMessage = "Description must be at least 3 characters long")]
        [MaxLength(100, ErrorMessage = "Description cannot be longer than 100 characters")]
        public string Description { get; set; }
        public Edition(int id,int year, string theme, string description)
        {
            Id = id;
            Year = year;
            Theme = theme;
            Description = description;
        }
        public Edition()
        {
        }
    }
}
