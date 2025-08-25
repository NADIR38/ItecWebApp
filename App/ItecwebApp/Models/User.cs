using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    public int User_Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Username { get; set; }

    public string Password_Hash { get; set; } // remove [Required]

    [NotMapped]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } // bind from form

    [Required]
    public string Email { get; set; }

    public int Role_Id { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    public string role_name { get; set; }
   

}
public class RegisterViewModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public string RoleName { get; set; }
}
