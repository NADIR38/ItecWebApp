using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel.DataAnnotations;

namespace ItecwebApp.Models
{
    public class CommitteMemeber
    {
        public int Id { get; set; }
  
            [Required]
            [StringLength(100, ErrorMessage = "Name canot be more than 100 letter")]
            public string name { get; set; }
        [Required]
        public string CommiteeName { get; set; }
        [Required]
        public string role_name { get; set; }


    }
    }
