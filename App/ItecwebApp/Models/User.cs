using System.ComponentModel.DataAnnotations;

namespace ItecwebApp.Models
{

    public class User
    {
        public int User_Id { get; set; }
        public string Username { get; set; }
        public string Password_Hash { get; set; }
        public string Email { get; set; }
        public int Role_Id { get; set; }
        public bool Is_Email_Confirmed { get; set; }  // ✅ new field
    }


    public class Role
        {
            public int Role_Id { get; set; }
            public string Role_Name { get; set; }
        }

        // For Login View
        public class LoginViewModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        // For Signup View
        public class SignupViewModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public int Role_Id { get; set; }
        }
    }


