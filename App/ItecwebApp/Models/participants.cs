using System.ComponentModel.DataAnnotations;

namespace ItecwebApp.Models
{
    public class Participants
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        public string role { get; set; }

        [StringLength(15, ErrorMessage = "Phone number cannot be longer than 15 characters.")]
       public string? phone { get; set; }
        [StringLength(200, ErrorMessage = "Institute cannot be longer than 200 characters.")]
        public string? institute { get; set; }
        [Required]
        [Range(2000, 2100, ErrorMessage = "Year must be between 2000 and 2100.")]
        public int year { get; set; }
        [Required]
        public string event_name { get; set; }
        [Required]
        public string payment_status { get; set; }
        [Required]
        [Range ( 0.01,double.MaxValue, ErrorMessage = "Fee amount must be greater than 0.")]
        public decimal feeamount { get; set; }

    }
}
