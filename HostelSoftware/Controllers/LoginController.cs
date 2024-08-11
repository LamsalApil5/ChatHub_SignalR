using Database;
using Entity.Tables;
using HostelSoftware.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace HostelSoftware.Controllers
{
    public class LoginController:Controller
    {
        private readonly MyDBContext _context;
        private readonly PasswordHasher<Users> _passwordHasher;

        public LoginController(MyDBContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Users>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == model.Username && u.IsActive == true);
                if (user != null)
                {
                    var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        // Create a claims identity and sign in
                        var claimsIdentity = new ClaimsIdentity(new[]
                        {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }, IdentityConstants.ApplicationScheme);

                        await HttpContext.SignInAsync(
                            IdentityConstants.ApplicationScheme,
                            new ClaimsPrincipal(claimsIdentity)
                        );

                        HttpContext.Session.SetInt32("UserId", user.Id);

                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "Invalid password.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users
                {
                    Fullname = model.Fullname,
                    Username = model.Username,
                    Email = model.Email,
                    Number = model.Number,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                // Hash the password before saving
                user.Password = _passwordHasher.HashPassword(user, model.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
