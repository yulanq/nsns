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




       

        // GET: Staff/Delete/{userId}
        [HttpGet("ConfirmDelete/{courseId}")]
        public async Task<IActionResult> ConfirmDelete(int courseId)
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


        [HttpPost("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int courseId)
        {
            try
            {
                var result = await _courseService.RemoveAsync(courseId);

                if (!result)
                {
                    TempData["ErrorMessage"] = "The course could not be deleted.";
                    return RedirectToAction("List");
                }

                TempData["SuccessMessage"] = "The course has been deleted successfully.";
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
            if (!ModelState.IsValid)
            {
                var specialties = await _specialtyRepository.GetAllAsync();
                ViewBag.SpecialtyList = specialties.Select(s => new SelectListItem
                {
                    Value = s.SpecialtyID.ToString(),
                    Text = s.Title
                }).ToList();
                return View();
            }

            try
            {
                var result = await _courseService.AddAsync(model.Title, model.Description, model.HourlyCost, model.IsActive, model.UserID,15);

                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Failed in adding the course info.");
                    var specialties = await _specialtyRepository.GetAllAsync();
                    ViewBag.SpecialtyList = specialties.Select(s => new SelectListItem
                    {
                        Value = s.SpecialtyID.ToString(),
                        Text = s.Title
                    }).ToList();
                    return View();
                }

                TempData["SuccessMessage"] = "Course info has been added successfully.";
                return RedirectToAction("List"); // Redirect to the course list page
            }
            catch (Exception ex)
            {
               
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                var specialties = await _specialtyRepository.GetAllAsync();
                ViewBag.SpecialtyList = specialties.Select(s => new SelectListItem
                {
                    Value = s.SpecialtyID.ToString(),
                    Text = s.Title
                }).ToList();
                return View(); 
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
        public async Task<IActionResult> Edit(int courseId, string title, string description, decimal hourlyCost, bool isActive/*, int userId, int updatedBy*/)
        {
           

            try
            {
                var result = await _courseService.UpdateAsync(courseId, title, description, hourlyCost, isActive /*userId, updatedBy*/);


                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update staff information.");
                    var course = await _courseService.GetAsync(courseId);
                    return View(course);
                }

                TempData["SuccessMessage"] = "Staff information updated successfully.";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                var course = await _courseService.GetAsync(courseId);
                return View(course);
            }
        }
    }   
}

