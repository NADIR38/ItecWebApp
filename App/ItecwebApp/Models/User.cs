using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    public int User_Id { get; set; }

    public string Name { get; set; }

    public string Username { get; set; }

    public string Password_Hash { get; set; } // remove [Required]


    public string? Password { get; set; } // bind from form

    public string Email { get; set; }

    public int Role_Id { get; set; }

    public string role_name { get; set; }
   

}
public class RegisterViewModel
{
    public int User_Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public string RoleName { get; set; }
}
public class EditUserViewModel
{
    public int User_Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public string RoleName { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; } // optional on edit
}
