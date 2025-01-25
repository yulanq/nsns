
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Models;

using System.Diagnostics;



namespace Web.Controllers.Authentication
{
    [Route("User")]
    //[ApiController]
    public class UserController: Controller
    {
        private readonly IUserService _userService;
        

        public UserController(IUserService userService)
        {
            _userService = userService;
        }




        //// POST: AddAdmin Action
        //[HttpPost("AddAdmin")]
        ////[HttpPost]
        //public async Task<IActionResult> AddAdmin(string email, string password)
        //{

          


        //    // Add admin using IUserService
        //    var result = false;
        //    try
        //    {
        //         result = await _userService.AddAdminAsync(email, password);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(String.Empty, ex.Message);
        //        return View();
        //    }

        //    if (!result)
        //    {
        //        ModelState.AddModelError(string.Empty, "Failed to add admin.");
        //        return View();
        //    }

        //    // Redirect to a success page or list of users
        //    return RedirectToAction("Index", "User");

            

        //}

        //// GET: AddAdmin View
        //[HttpGet("AddAdmin")]
        ////[HttpGet]
        //public IActionResult AddAdmin()
        //{
        //    return View();

        //}

       
    }
}
