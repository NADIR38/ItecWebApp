using ItecwebApp.DAL;
using ItecwebApp.Helpers;
using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static User;

namespace ItecwebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserDAL idl;

        public AccountController(IUserDAL idl)
        {
            this.idl = idl;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // default role = Student
            string roleToAssign = "Student";

            // If Admin is logged in, allow them to pick Faculty/Admin
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin") && !string.IsNullOrEmpty(model.RoleName))
            {
                roleToAssign = model.RoleName;
            }

            var user = new User
            {
                Name = model.Name,
                Username = model.Username,
                Email = model.Email,
                role_name = roleToAssign, // safe role assignment
                Password_Hash = PasswordHelper.HashPassword(model.Password)
            };

            if (idl.RegisterUser(user))
            {
                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Registration failed. Username or Email might already exist.");
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Login(User u)
        {
            try
            {
                var username = u.Username;
                var password = u.Password; // <-- plain password

                var user = idl.GetUserByUsername(username);

                if (user != null && PasswordHelper.VerifyPassword(password, user.Password_Hash))
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.role_name) // actual role
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        claimsPrincipal,
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddHours(2)
                        });

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
