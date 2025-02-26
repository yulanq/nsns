
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Models;

using System.Diagnostics;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.ViewModels;



namespace Web.Controllers.User
{
    [Route("Coach")]
    //[ApiController]
    public class CoachController : Controller
    {
        private readonly ICoachService _coachService;
        private readonly ICityService _cityService;
        private readonly ISpecialtyService _specialtyService;
        private readonly ICourseEnrollmentService _courseEnrollmentService;
        private readonly ICourseService _courseService;

        public CoachController(ICoachService coachService, ICityService cityService, ISpecialtyService specialtyService, ICourseEnrollmentService courseEnrollmentService, ICourseService courseService)
        {
            _coachService = coachService;
            _cityService = cityService;
            _specialtyService = specialtyService;
            _courseEnrollmentService = courseEnrollmentService;
            _courseService = courseService;
        }


        // POST: Add Staff Action
        [HttpPost("Add")]
        //[HttpPost]
        public async Task<IActionResult> Add(string name, string email, string password, int specialtyId, string gender, string phone, string wechat, int cityId)
        {

           
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var result = await _coachService.AddAsync(name, email, password, specialtyId, gender, phone, wechat, cityId);
                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Failed in adding the coach info.");

                   
                    // Repopulate CityList for the dropdown if validation fails

                    var cities = await _cityService.GetAllAsync(); // Replace with your data fetching logic
                    ViewBag.CityList = cities.Select(c => new SelectListItem
                    {
                        Value = c.CityID.ToString(),
                        Text = c.Name
                    }).ToList();

                    var specialties = await _specialtyService.GetAllAsync(); // Replace with your data fetching logic
                    ViewBag.SpecialtyList = specialties.Select(c => new SelectListItem
                    {
                        Value = c.SpecialtyID.ToString(),
                        Text = c.Title
                    }).ToList();


                    return View();
                }
                TempData["SuccessMessage"] = "Coach info has been added successfully.";
                return RedirectToAction("List"); // Redirect to the coach list page


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, $"Error: {ex.Message}");
                
                // Repopulate CityList for the dropdown if validation fails

                var cities = await _cityService.GetAllAsync(); // Replace with your data fetching logic
                ViewBag.CityList = cities.Select(c => new SelectListItem
                {
                    Value = c.CityID.ToString(),
                    Text = c.Name
                }).ToList();

                var specialties = await _specialtyService.GetAllAsync(); // Replace with your data fetching logic
                ViewBag.SpecialtyList = specialties.Select(c => new SelectListItem
                {
                    Value = c.SpecialtyID.ToString(),
                    Text = c.Title
                }).ToList();

                  
                return View();
            }

            


        }

        // GET: Add View
        [HttpGet("Add")]
        //[HttpGet]
        public async Task<IActionResult> AddAsync()
        {
            var cities = await _cityService.GetAllAsync(); // Replace with your data fetching logic
            ViewBag.CityList = cities.Select(c => new SelectListItem
            {
                Value = c.CityID.ToString(),
                Text = c.Name
            }).ToList();

            var specialties = await _specialtyService.GetAllAsync(); // Replace with your data fetching logic
            ViewBag.SpecialtyList = specialties.Select(c => new SelectListItem
            {
                Value = c.SpecialtyID.ToString(),
                Text = c.Title
            }).ToList();


            return View();

        }


       




        // GET: Coach/Delete/{userId}
        [HttpGet("ConfirmDelete/{userId}")]
        public async Task<IActionResult> ConfirmDelete(int userId)
        {
            // Fetch the staff details from the database
            var coach = await _coachService.GetAsync(userId);
            if (coach == null)
            {
                return NotFound();
            }

            // Pass the staff details to the Delete.cshtml view
            return View(coach);
        }


        [HttpPost("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int userId)
        {
            try
            {
                var result = await _coachService.RemoveAsync(userId);

                if (!result)
                {
                    TempData["ErrorMessage"] = "The coach member could not be deleted.";
                    return RedirectToAction("List");
                }

                TempData["SuccessMessage"] = "Coach member has been deleted successfully.";
                return RedirectToAction("List"); // Redirect to the coach list page
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("List");
            }
        }





        // GET: Add View
        [HttpGet("List")]
        //[HttpGet]
        public async Task<IActionResult> List()
        {

            var coachList = await _coachService.GetAllAsync();
            return View(coachList); // Ensure there is a corresponding List.cshtml in Views/Staff

           
        }



        // GET: Edit View
        [HttpGet("Edit/{userId}")]
        //[HttpGet]
        public async Task<IActionResult> Edit(int userId)
        {
            //Fetch the staff details from the database


           var coach = await _coachService.GetAsync(userId);

            if (coach == null)
            {
                return NotFound();
            }

            var cities = await _cityService.GetAllAsync(); // Replace with your data fetching logic



            ViewBag.CityList = cities.Select(c => new SelectListItem
            {
                Value = c.CityID.ToString(),
                Text = c.Name,
                Selected = c.CityID == coach.CityID
            }).ToList();


            var specialties = await _specialtyService.GetAllAsync(); // Replace with your data fetching logic

            ViewBag.SpecialtyList = specialties.Select(s => new SelectListItem
            {
                Value = s.SpecialtyID.ToString(),
                Text = s.Title,
                Selected = s.SpecialtyID == coach.SpecialtyID
            }).ToList();

            // Pass the coach details to the Edit.cshtml view
            return View(coach);
            //return LoadPageForEdit(userId);

        }


        [HttpPost("Edit/{userId}")]
        [ValidateAntiForgeryToken]

       
        public async Task<IActionResult> Edit(int userId, string name, string email, /*string password,*/int specialtyId, string gender, string phone, string wechat, int cityId)
        {
           

            try
            {
                var result = await _coachService.UpdateAsync(userId, name, email, /*password,*/specialtyId, gender, phone, wechat, cityId);


                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update coach information.");
                    var coach = await _coachService.GetAsync(userId);

                    if (coach == null)
                    {
                        return NotFound();
                    }

                    var cities = await _cityService.GetAllAsync(); // Replace with your data fetching logic



                    ViewBag.CityList = cities.Select(c => new SelectListItem
                    {
                        Value = c.CityID.ToString(),
                        Text = c.Name,
                        Selected = c.CityID == coach.CityID
                    }).ToList();


                    var specialties = await _specialtyService.GetAllAsync(); // Replace with your data fetching logic

                    ViewBag.SpecialtyList = specialties.Select(s => new SelectListItem
                    {
                        Value = s.SpecialtyID.ToString(),
                        Text = s.Title,
                        Selected = s.SpecialtyID == coach.SpecialtyID
                    }).ToList();

                    // Pass the coach details to the Edit.cshtml view
                    return View(coach);
                }

                TempData["SuccessMessage"] = "coach information updated successfully.";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                //TempData["ErrorMessage"] = $"Error: {ex.Message}";
                var coach = await _coachService.GetAsync(userId);

                if (coach == null)
                {
                    return NotFound();
                }

                var cities = await _cityService.GetAllAsync(); // Replace with your data fetching logic



                ViewBag.CityList = cities.Select(c => new SelectListItem
                {
                    Value = c.CityID.ToString(),
                    Text = c.Name,
                    Selected = c.CityID == coach.CityID
                }).ToList();


                var specialties = await _specialtyService.GetAllAsync(); // Replace with your data fetching logic

                ViewBag.SpecialtyList = specialties.Select(s => new SelectListItem
                {
                    Value = s.SpecialtyID.ToString(),
                    Text = s.Title,
                    Selected = s.SpecialtyID == coach.SpecialtyID
                }).ToList();

                // Pass the coach details to the Edit.cshtml view
                return View(coach);
            }
        }

        [HttpGet("ManageSchedules")]
        public async Task<IActionResult> ManageSchedules()
        {
            int coachId = 10;  //GetLoggedInCoachId();
            // ✅ Get children who are enrolled in the coach's courses
            var children = await _courseEnrollmentService.GetRegisteredChildrenByCoachAsync(coachId);
            
           
            // ✅ Get courses assigned to the coach
            var course = await _courseService.GetActiveCourseByCoachAsync(coachId);

            // ✅ Check if SelectedChildId exists in session
            int? selectedChildId = HttpContext.Session.GetInt32("SelectedChildId");

            // ✅ If session is empty, don't select any child at first
            if (!selectedChildId.HasValue && children.Any())
            {
                selectedChildId = null; // No child is preselected
            }

            // ✅ Get schedules only if a child is selected
            List<CourseEnrollment> schedules = new List<CourseEnrollment>();
            if (selectedChildId.HasValue)
            {
                schedules = (List<CourseEnrollment>)await _courseEnrollmentService.GetSchedulesByCourseChildAsync(course.CourseID, selectedChildId.Value);
            }


            var model = new ManageSchedulesViewModel
            {
                SelectedChildId = selectedChildId,
                Children = children.Select(c => new SelectListItem { Value = c.ChildID.ToString(), Text = c.Name }).ToList(),
                Course = course,
                Schedules = schedules
            };

            return View(model);
        }


        [HttpPost("ScheduleCourse")]
        public async Task<IActionResult> ScheduleCourse(int childId, int courseId, DateTime scheduledAt, decimal scheduledHours)
        {
            int coachId = 10; // GetLoggedInCoachId(); // Replace with actual logic to get coach ID
            if (childId == 0)
            {
                TempData["ValidationMessage"] = "Please select a child before scheduling a course.";
                return RedirectToAction("ManageSchedules");
            }

            //Store the SelectedChildId into session
            HttpContext.Session.SetInt32("SelectedChildId", childId);

            bool result = await _courseEnrollmentService.ScheduleCourseAsync(childId, courseId, scheduledAt, scheduledHours, coachId);

            if (result)
            {
                TempData["SuccessMessage"] = "Course scheduled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to schedule the course.";
            }

            return RedirectToAction("ManageSchedules");
        }

        [HttpGet("GetSchedules")]
        public async Task<IActionResult> GetSchedules(int courseId, int childId)
        {
            var schedules = await _courseEnrollmentService.GetSchedulesByCourseChildAsync(courseId, childId);
            var response = schedules.Select(s => new
            {
                enrollmentID = s.EnrollmentID,
                courseTitle = s.Course.Title,
                scheduledAt = s.ScheduledAt?.ToString("yyyy-MM-dd HH:mm"),
                scheduledHours = s.ScheduledHours
            }).ToList();

            return Json(response);
        }

        [HttpPost("DeleteSchedule")]
        public async Task<IActionResult> DeleteSchedule(int enrollmentId)
        {
            bool result = await _courseEnrollmentService.RemoveScheduleAsync(enrollmentId);

            if (result)
            {
                TempData["SuccessMessage"] = "Schedule deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete the schedule.";
            }

            return RedirectToAction("ManageSchedules");
        }




    }
}

