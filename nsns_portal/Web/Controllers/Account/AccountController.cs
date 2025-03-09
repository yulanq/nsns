using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Core.Services;
using Core.Models;
using Core.Interfaces;

namespace Web.Controllers.Account
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IUserRegistrationService _userRegistrationService;

        public AccountController(IUserRegistrationService userRegistrationService)
        {
            _userRegistrationService = userRegistrationService;
        }

        // Show Registration Form (GET)
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        // Handle Form Submission (POST)
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return View(model); // Return form with validation errors

            var result = await _userRegistrationService.RegisterUserAsync(model.Email, model.Password, model.Role);

            if (result)
                return RedirectToAction("Login", "Account"); // Redirect to login page

            ModelState.AddModelError("", "Registration failed. Please try again.");
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
