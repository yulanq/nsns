using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Courses
{
    [Route("Course")]
    public class CourseController: Controller
    {
        
        //[ApiController]
     
            private readonly ICourseService _courseService;
           

            public CourseController(ICourseService courseService)
            {
            _courseService = courseService;
             
            }




            // POST: Add Staff Action
            [HttpPost("Add")]
            //[HttpPost]
            public async Task<IActionResult> Add(string title, string description, decimal hourlyCost, bool active, int coachId, int createdBy)
            {
            //string title, string description, decimal hourlyCost, bool active, int coachId, int createdBy

                // Add admin using IUserService
                var result = false;
                try
                {
                    result = await _courseService.AddAsync(title,  description,  hourlyCost,  active,  coachId, createdBy);
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

                TempData["SuccessMessage"] = "The staff member has been added.";
                return RedirectToAction("List", "Staff");

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
