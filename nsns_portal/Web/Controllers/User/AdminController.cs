
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Models;

using System.Diagnostics;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;



namespace Web.Controllers.User
{
    [Route("Admin")]
    //[ApiController]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
          
        }




        // POST: Add Staff Action
        [HttpPost("Add")]
        //[HttpPost]
        public async Task<IActionResult> Add(string name, string email, string password, string phone, string wechat)
        {

            // Add admin using IUserService
            var result = false;
            try
            {
                result = await _adminService.AddAsync( name, email, password,  phone,  wechat);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
               
            }

            if (!result)
            {
                return View(); // Return view with errors for correction

            }

            // Redirect to a success page or list of users

            TempData["SuccessMessage"] = "The admin member has been added.";
            return RedirectToAction("List", "Admin");

        }

        // GET: Add View
        [HttpGet("Add")]
        //[HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();

        }

        // GET: Staff/Delete/{userId}
        [HttpGet("ConfirmDelete/{userId}")]
        public async Task<IActionResult> ConfirmDelete(int userId)
        {
            // Fetch the staff details from the database
            var admin = await _adminService.GetAsync(userId);
            if (admin == null)
            {
                return NotFound();
            }

            // Pass the staff details to the Delete.cshtml view
            return View(admin);
        }


        [HttpPost("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int userId)
        {
            try
            {
                var result = await _adminService.RemoveAsync(userId);

                if (!result)
                {
                    TempData["ErrorMessage"] = "The admin member could not be deleted.";
                    return RedirectToAction("List");
                }

                TempData["SuccessMessage"] = "Admin member has been deleted successfully.";
                return RedirectToAction("List"); // Redirect to the staff list page
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            // If delete fails, reload the confirmation page
            var admin = await _adminService.RemoveAsync(userId);
            return View(admin);
        }



        // GET: Add View
        [HttpGet("List")]
        //[HttpGet]
        public async Task<IActionResult> List()
        {

            var adminList = await _adminService.GetAllAsync();
            return View(adminList); // Ensure there is a corresponding List.cshtml in Views/Staff

        }
       

        // GET: Edit View
        [HttpGet("Edit/{userId}")]
        //[HttpGet]
        public async Task<IActionResult> Edit(int userId)
        {
            // Fetch the staff details from the database
            var admin = await _adminService.GetAsync(userId);
            if (admin == null)
            {
                return NotFound();
            }

            // Pass the staff details to the Delete.cshtml view
            return View(admin);

        }


        [HttpPost("Edit/{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int userId, string name, string email, /*string password,*/ string phone, string wechat)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _adminService.UpdateAsync(userId, name, email, /*password,*/ phone, wechat);


            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to update staff information.");
                return View();
            }

            TempData["SuccessMessage"] = "Admin information updated successfully.";
            return RedirectToAction("List");
        }
    }
}
