using Core.Interfaces;
using Core.Models;
using Core.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Web.ViewModels;

namespace Web.Controllers.Courses
{
    [Route("Course")]
    public class CourseController : Controller
    {

        //[ApiController]

        private readonly ICourseService _courseService;
        private readonly ICoachService _coachService;
        private readonly IRepository<Specialty> _specialtyRepository;


        public CourseController(ICourseService courseService, ICoachService coachService, IRepository<Specialty> specialtyRepository)
        {
            _courseService = courseService;
            _coachService = coachService;
            _specialtyRepository = specialtyRepository;

        }




        // POST: Add Staff Action
        //[HttpPost("Add")]
        ////[HttpPost]
        //public async Task<IActionResult> Add(string title, string description, decimal hourlyCost, bool active, int coachId, int createdBy)
        //{
        ////string title, string description, decimal hourlyCost, bool active, int coachId, int createdBy

        //    // Add admin using IUserService
        //    var result = false;
        //    try
        //    {
        //        result = await _courseService.AddAsync(title,  description,  hourlyCost,  active,  coachId, createdBy);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(String.Empty, ex.Message);

        //    }

        //    if (!result)
        //    {
        //        return View(); // Return view with errors for correction

        //    }

        //    // Redirect to a success page or list of users

        //    TempData["SuccessMessage"] = "The staff member has been added.";
        //    return RedirectToAction("List", "Staff");

        //}

        //// GET: Add View
        //[HttpGet("Add")]
        ////[HttpGet]
        //public async Task<IActionResult> Add()
        //{
        //    return View();

        //}

        // GET: Staff/Delete/{userId}
        [HttpGet("ConfirmDelete/{userId}")]
        public async Task<IActionResult> ConfirmDelete(int courseId)
        {
            // Fetch the staff details from the database
            var staff = await _courseService.GetAsync(courseId);
            if (staff == null)
            {
                return NotFound();
            }

            // Pass the staff details to the Delete.cshtml view
            return View(staff);
        }


        [HttpPost("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int courseId)
        {
            try
            {
                var result = await _courseService.RemoveAsync(courseId);

                if (!result)
                {
                    TempData["ErrorMessage"] = "The staff member could not be deleted.";
                    return RedirectToAction("List");
                }

                TempData["SuccessMessage"] = "Staff member has been deleted successfully.";
                return RedirectToAction("List"); // Redirect to the course list page
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("List"); // Redirect to the course list page
            }

            // If delete fails, reload the confirmation page


        }



        // GET: Add View
        [HttpGet("List")]
        //[HttpGet]
        public async Task<IActionResult> List()
        {

            var courseList = await _courseService.GetAllAsync();
            return View(courseList); // Ensure there is a corresponding List.cshtml in Views/Staff

        }

        [HttpGet("GetCoachesBySpecialty")]
        public async Task<IActionResult> GetCoachesBySpecialty(int specialtyId)
        {
            var coaches = await _coachService.GetCoachesBySpecailtyAsync(specialtyId);
            return Json(coaches.Select(c => new { c.UserID, c.Name }));
        }

        [HttpGet("Add")]
        public async Task<IActionResult> Add()
        {
            var specialties = await _specialtyRepository.GetAllAsync();
            ViewBag.SpecialtyList = specialties.Select(s => new SelectListItem
            {
                Value = s.SpecialtyID.ToString(),
                Text = s.Title
            }).ToList();

            return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(CourseViewModel model)
        {

            // Save course to database
            //var course = new Course
            //{
            //    Title = model.Title,
            //    Description = model.Description,
            //    HourlyCost = model.HourlyCost,
            //    IsActive = model.IsActive,
            //    CoachID = model.CoachID
            //};
            try
            {
                var result = await _courseService.AddAsync(model.Title, model.Description, model.HourlyCost, model.IsActive, model.UserID,15);

                if (!result)
                {
                    TempData["ErrorMessage"] = "Failed in adding the course info.";

                    // Reload specialties in case of error

                    //var specialties = await _specialtyRepository.GetAllAsync();
                    //ViewBag.SpecialtyList = specialties.Select(s => new SelectListItem
                    //{
                    //    Value = s.SpecialtyID.ToString(),
                    //    Text = s.Title
                    //}).ToList();
                    //return View(model);
                    return RedirectToAction("List"); // Redirect to the course list page
                }

                TempData["SuccessMessage"] = "Course info has been added successfully.";
                return RedirectToAction("List"); // Redirect to the course list page
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("List"); // Redirect to the course list page
            }




                


                

                
            }
            










       


        // GET: Edit View
        [HttpGet("Edit/{courseId}")]
        //[HttpGet]
        public async Task<IActionResult> Edit(int courseId)
        {
            // Fetch the staff details from the database
            var course = await _courseService.GetAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            // Pass the staff details to the Delete.cshtml view
            return View(course);

        }


        [HttpPost("Edit/{courseId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int courseId, string title, string description, decimal hourlyCost, bool active, int coachId, int updatedBy)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _courseService.UpdateAsync(courseId, title, description, hourlyCost, active, coachId, updatedBy);


            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to update staff information.");
                return View();
            }

            TempData["SuccessMessage"] = "Staff information updated successfully.";
            return RedirectToAction("List");

        }
    }   
}

