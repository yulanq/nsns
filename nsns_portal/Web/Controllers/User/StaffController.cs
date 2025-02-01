
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
    [Route("Staff")]
    //[ApiController]
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Specialty> _specialtyRepository;

        public StaffController(IStaffService staffService, IRepository<City> cityRepository, IRepository<Specialty> specialtyRepository)
        {
            _staffService = staffService;
            _cityRepository = cityRepository;
            _specialtyRepository = specialtyRepository;
        }




        // POST: Add Staff Action
        [HttpPost("Add")]
        //[HttpPost]
        public async Task<IActionResult> Add(string name, string email, string password, string phone, string wechat)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            
            try
            {
                var result = await _staffService.AddAsync( name, email, password,  phone,  wechat);
                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Failed in adding the staff info.");
                    return View();
                }

                TempData["SuccessMessage"] = "Staff info has been added successfully.";
                return RedirectToAction("List"); // Redirect to the staff list page


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, $"Error: {ex.Message}");
                return View();

            }
           

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
            var staff = await _staffService.GetAsync(userId);
            if (staff == null)
            {
                return NotFound();
            }

            // Pass the staff details to the Delete.cshtml view
            return View(staff);
        }


        [HttpPost("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int userId)
        {
            try
            {
                var result = await _staffService.RemoveAsync(userId);

                if (!result)
                {
                    TempData["ErrorMessage"] = "The staff member could not be deleted.";
                    return RedirectToAction("List");
                }

                TempData["SuccessMessage"] = "Staff member has been deleted successfully.";
                return RedirectToAction("List"); // Redirect to the staff list page
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("List"); // Redirect to the staff list page
            }
        }

            


        // GET: Add View
        [HttpGet("List")]
        //[HttpGet]
        public async Task<IActionResult> List()
        {

            var staffList = await _staffService.GetAllAsync();
            return View(staffList); // Ensure there is a corresponding List.cshtml in Views/Staff

        }
       

        // GET: Edit View
        [HttpGet("Edit/{userId}")]
        //[HttpGet]
        public async Task<IActionResult> Edit(int userId)
        {
            // Fetch the staff details from the database
            var staff = await _staffService.GetAsync(userId);
            if (staff == null)
            {
                return NotFound();
            }

            // Pass the staff details to the Delete.cshtml view
            return View(staff);

        }


        [HttpPost("Edit/{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int userId, string name, string email, /*string password,*/ string phone, string wechat)
        {
            

            try
            {
                var result = await _staffService.UpdateAsync(userId, name, email, /*password,*/ phone, wechat);


                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update staff information.");
                    var staff = await _staffService.GetAsync(userId);
                    return View(staff);
                }

                TempData["SuccessMessage"] = "Staff information updated successfully.";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                var staff = await _staffService.GetAsync(userId);
                return View(staff);
            }

           
        }
       
    }
}
