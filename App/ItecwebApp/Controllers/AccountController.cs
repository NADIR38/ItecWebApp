using ItecwebApp.DAL;
using ItecwebApp.Helpers;
using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static User;

namespace ItecwebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserDAL idl;
        private readonly EmailService _emailService;

        public AccountController(IUserDAL idl, EmailService emailService)
        {
            this.idl = idl;
            _emailService = emailService;
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
            if (roleToAssign == "Student" && !User.IsInRole("Admin"))
            {

                string otp = new Random().Next(100000, 999999).ToString();
                HttpContext.Session.SetString("otp", otp);
                HttpContext.Session.SetString("email", model.Email);
                HttpContext.Session.SetString("username", model.Username);
                HttpContext.Session.SetString("password", PasswordHelper.HashPassword(model.Password));
                HttpContext.Session.SetString("name", model.Name);
                // Send OTP to user's email
                _emailService.SendEmail(model.Email, "Verify Your Email", $"Your OTP is: <b>{otp}</b>");

                return RedirectToAction("VerifyOtp");
            }
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
        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl = "/")
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider); // "Google" or "GitHub"
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            // Extract claims
            var email = User.FindFirstValue(ClaimTypes.Email);
            var name = User.FindFirstValue(ClaimTypes.Name) ?? email;

            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "External login failed: no email returned.";
                return RedirectToAction("Login");
            }

            var existingUser = idl.GetUserByEmail(email);

            if (existingUser == null)
            {
                var newUser = new User
                {
                    Name = name,
                    Username = email,
                    Email = email,
                    role_name = "Student",
                    Password_Hash = null
                };

                idl.RegisterUser(newUser);
                existingUser = idl.GetUserByEmail(email);
            }

            // Build claims for your system
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, existingUser.Username),
        new Claim(ClaimTypes.NameIdentifier, existingUser.User_Id.ToString()),
        new Claim(ClaimTypes.Email, existingUser.Email),
        new Claim(ClaimTypes.Role, existingUser.role_name)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return LocalRedirect(returnUrl);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var list = idl.GetAllUsers();
            return View(list);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (idl.DeleteUser(id))
                {
                    TempData["SuccessMessage"] = "User deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete user. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = idl.GetUserById(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            var model = new EditUserViewModel
            {
                User_Id = user.User_Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email,
                RoleName = user.role_name,
                Password = "" // leave blank
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = idl.GetUserById(model.User_Id);
            if (existingUser == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            existingUser.Name = model.Name;
            existingUser.Username = model.Username;
            existingUser.Email = model.Email;
            existingUser.role_name = model.RoleName;

            // Update password only if provided
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                existingUser.Password_Hash = PasswordHelper.HashPassword(model.Password);
            }

            if (idl.Updateuser(existingUser))
                TempData["SuccessMessage"] = "User updated successfully!";
            else
                TempData["ErrorMessage"] = "Failed to update user.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyOtp(string otp)
        {
            string savedopt = HttpContext.Session.GetString("otp");
            if (!string.IsNullOrWhiteSpace(otp) && otp == savedopt)
            {
                var user = new User
                {
                    Name = HttpContext.Session.GetString("name"),
                    Email = HttpContext.Session.GetString("email"),
                    Username = HttpContext.Session.GetString("username"),
                    role_name = "Student", // safe role assignment
                    Password_Hash =(HttpContext.Session.GetString("password"))
                };

                idl.RegisterUser(user);

                HttpContext.Session.Clear();
                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }

            ViewBag.error = "Invalid OTP. Please try again.";
            return View();
        }
    }
}