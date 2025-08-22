using ItecwebApp.BL;
using ItecwebApp.DAL;
using ItecwebApp.Helpers;
using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Security.Claims;

namespace ItecwebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserDAL _userDal;

        public AccountController(IUserDAL userDal)
        {
            _userDal = userDal;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userDal.GetUserByUsername(model.Username); // fetch user with hash
                if (user != null && PasswordHelper.VerifyPassword(model.Password, user.Password_Hash))
                {
                    if (!user.Is_Email_Confirmed)
                    {
                        ModelState.AddModelError("", "Please confirm your email before logging in.");
                        return View(model);
                    }

                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetInt32("Role_Id", user.Role_Id);

                    return user.Role_Id switch
                    {
                        1 => RedirectToAction("Index", "Home"),
                        2 => RedirectToAction("Index", "Members"),
                        3 => RedirectToAction("Index", "Participants"),
                        _ => RedirectToAction("Login", "Account")
                    };
                }

                ModelState.AddModelError("", "Invalid credentials.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View(new SignupViewModel());
        }

        [HttpPost]
        public IActionResult Signup(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password_Hash = PasswordHelper.HashPassword(model.Password),
                    Role_Id = model.Role_Id
                };

                if (_userDal.RegisterUser(newUser))
                {
                    // Generate token (simple example: base64 of email)
                    var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(newUser.Email));

                    var confirmLink = Url.Action("ConfirmEmail", "Account", new { email = newUser.Email, token = token },
                                                 Request.Scheme);

                    EmailSender.Send(newUser.Email, "Confirm your account",
                        $"Please confirm your email by clicking <a href='{confirmLink}'>here</a>");

                    return View("CheckYourEmail"); // Create a view to tell user to check inbox
                }
                ModelState.AddModelError("", "Error while registering.");
            }
            return View(model);
        }


        public IActionResult ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            var decodedEmail = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token));

            if (decodedEmail == email)
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE users SET is_email_confirmed = 1 WHERE email = @Email";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.ExecuteNonQuery();
                    }
                }
                return View("ConfirmEmailSuccess"); // Create a simple success view
            }

            return View("Error");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}