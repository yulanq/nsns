
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Models;
using Core.ViewModels;

using System.Diagnostics;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;



namespace Web.Controllers.User
{
    [Route("Child")]
    //[ApiController]
    public class ChildController : Controller
    {
        private readonly IChildService _childService;
        private readonly IParentService _parentService;
        private readonly ICityService _cityService;
        private readonly IParentChildService _parentChildService;
        private readonly ICourseService _courseService;
        private readonly ICourseEnrollmentService _courseEnrollmentService;
        private readonly ISpecialtyService _specialtyService;
        private readonly IActivityEnrollmentService _activityEnrollmentService;
        private readonly IActivityService _activityService;
        private readonly IPaymentService _paymentService;

        public ChildController(IChildService childService, IParentService parentService, ICityService cityService, IParentChildService parentChildService, ICourseService courseService, ISpecialtyService specialtyService, IActivityService activityService, ICourseEnrollmentService courseEnrollmentService, IActivityEnrollmentService activityEnrollmentService, IPaymentService paymentService)
        {
            _childService = childService;
            _parentService = parentService;
            _cityService = cityService;
            _parentChildService = parentChildService;
            _courseService = courseService;
            _specialtyService = specialtyService;
            _courseEnrollmentService = courseEnrollmentService;
            _activityService = activityService;
            _activityEnrollmentService = activityEnrollmentService;
            _paymentService = paymentService;
        }

        // ✅ Helper method to get City List
        private async Task<List<SelectListItem>> GetCityList()
        {
            return (await _cityService.GetAllAsync())
                .Select(c => new SelectListItem { Value = c.CityID.ToString(), Text = c.Name })
                .ToList();
        }


        private async Task<List<SelectListItem>> GetCityList(Child child)
        {

            var cities = await _cityService.GetAllAsync();

            return cities.Select(c => new SelectListItem
            {
                Value = c.CityID.ToString(),
                Text = c.Name,
                Selected = c.CityID == child.CityID
            }).ToList();
        }


        [HttpGet("List")]
        // ✅ List all children
        public async Task<IActionResult> List()
        {
            var children = await _childService.GetAllAsync();
            return View(children);


            //var children = await _childService.GetAllAsync();
            //ViewBag.ParentList = (await _parentService.GetAllParentsAsync())
            //    .Select(p => new SelectListItem { Value = p.ParentID.ToString(), Text = p.Name })
            //    .ToList();

            //return View(children);

        }


        // GET: Add View
        [HttpGet("Add")]
        //[HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.CityList = await GetCityList();

            return View();
        }


        [HttpPost("Add")]
        public async Task<IActionResult> Add(Child child)
        {
            City city = child.CityID.HasValue ? await _cityService.GetAsync(child.CityID.Value) : null;
            child.City = city;
            child.Role = "Child";
            try
            {
                var result = await _childService.AddAsync(child);
                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Failed in adding the coach info.");


                    // Repopulate CityList for the dropdown if validation fails

                    ViewBag.CityList = await GetCityList();
                    return View();
                }
                TempData["SuccessMessage"] = "Child info has been added successfully.";
                return RedirectToAction("List"); // Redirect to the child list page

            }
            
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, $"Error: {ex.Message}");
                // Repopulate CityList for the dropdown if validation fails
                ViewBag.CityList = await GetCityList();
                return View();
            }
        }


        // ✅ GET: Show Edit form with Child data
        [HttpGet("Edit/{userId}")]
        public async Task<IActionResult> Edit(int userId)
        {
            var child = await _childService.GetAsync(userId);
            if (child == null)
            {
                TempData["ErrorMessage"] = "Child not found.";
                return RedirectToAction("List");
            }

            ViewBag.CityList = await GetCityList(child);
            return View(child);
        }

       

        [HttpPost("Edit/{userId}")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int userId, string name, DateTime birthDate, string gender, int cityId, string email/*, string password*/)
        {


            try
            {
                var result = await _childService.UpdateAsync(userId, name, birthDate, gender, cityId, email/*, string password*/);


                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update child information.");
                    var child = await _childService.GetAsync(userId);

                    if (child == null)
                    {
                        return NotFound();
                    }

                   


                    ViewBag.CityList = await GetCityList(child);

                    // Pass the coach details to the Edit.cshtml view
                    return View(child);
                }

                TempData["SuccessMessage"] = "Child information updated successfully.";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                //TempData["ErrorMessage"] = $"Error: {ex.Message}";
                var child = await _childService.GetAsync(userId);

                if (child == null)
                {
                    return NotFound();
                }

               
            
                ViewBag.CityList = await GetCityList(child);



                // Pass the child details to the Edit.cshtml view
                return View(child);
            }
        }







        // ✅ GET: Confirm delete page
        [HttpGet("ConfirmDelete/{userId}")]
        public async Task<IActionResult> ConfirmDelete(int userId)
        {
            var child = await _childService.GetAsync(userId);
            if (child == null)
            {
                TempData["ErrorMessage"] = "Child not found.";
                return RedirectToAction("List");
            }

            return View(child);
        }

        // ✅ POST: Delete Child
        [HttpPost("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int userId)
        {
            var success = await _childService.RemoveAsync(userId);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to delete child.";
            }
            else
            {
                TempData["SuccessMessage"] = "Child deleted successfully.";
            }

            return RedirectToAction("List");
        }



        [HttpGet("ManageParents/{childId}")]
        public async Task<IActionResult> ManageParents(int childId)
        {
            var child = await _childService.GetChildByIdAsync(childId);
            if (child == null)
            {
                TempData["ErrorMessage"] = "Child not found.";
                return RedirectToAction("List");
            }

            var parents = await _parentChildService.GetParentsByChildIdAsync(childId);


            var model = new ManageParentsViewModel
            {
                Child = child,
                Parents = parents
            };

            return View(model);
        }


        [HttpPost("AddParentToChild")]
        public async Task<IActionResult> AddParentToChild(int childId, string parentName, string relationship)
        {
            try
            {
                // ✅ 1. Create a new Parent object
                var newParent = new Parent
                {
                    Name = parentName,
                    CreatedBy = 1, // Assume the user ID of admin/creator
                    CreatedDate = DateTime.UtcNow
                };

                // ✅ 2. Save the parent in the database
                var parentId = await _parentService.AddAndReturnIdAsync(newParent);
                if (parentId == 0)
                {
                    TempData["ErrorMessage"] = "Failed to add parent.";
                    return RedirectToAction("ManageParents", new { childId });
                }


                var success = await _parentChildService.AddParentToChild(parentId, newParent, childId, relationship, 1); // Assuming CreatedBy = 1
                if (!success)
                {
                    TempData["ErrorMessage"] = "Failed to add parent to child.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Parent added to child successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("ManageParents", new { childId });
        }


        [HttpPost("RemoveParentFromChild")]
        public async Task<IActionResult> RemoveParentFromChild(int parentChildId, int childId, int parentId)
        {
            try
            {
                
                var success = await _parentChildService.RemoveParentFromChild(parentChildId);
                success = await _parentService.DeleteAsync(parentId);
                if (!success)
                {
                    TempData["ErrorMessage"] = "Failed to remove parent.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Parent removed successfully.";
                }

                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("ManageParents", new { childId });
        }

        [HttpGet("ManageRegistrations/{userId}")]
        public async Task<IActionResult> ManageRegistrations(int userId)
        {
            var child = await _childService.GetAsync(userId);
            if (child == null)
            {
                TempData["ErrorMessage"] = "Child not found.";
                return RedirectToAction("List"); // Redirect to child list page if not found
            }

            var courseEnrollments = await _courseEnrollmentService.GetRegisteredEnrollmentsByChildAsync(userId);
            var specialties = await _specialtyService.GetAllAsync();

            ViewBag.SpecialtyList = specialties.Select(s => new SelectListItem
            {
                Value = s.SpecialtyID.ToString(),
                Text = s.Title
            }).ToList();

            var activityEnrollments = await _activityEnrollmentService.GetRegisteredEnrollmentsByChildAsync(userId);
            var activities = await _activityService.GetAllActiveAsync();

            ViewBag.ActivityList = activities.Select(a => new SelectListItem
            {
                Value = a.ActivityID.ToString(),
                Text = a.Title
            }).ToList();

            return View("ManageRegistrations", new ManageRegisterationsViewModel
            {
                Child = child,
                CourseEnrollments = courseEnrollments,
                ActivityEnrollments = activityEnrollments
            });
        }


        [HttpGet("GetCoursesBySpecialty")]
        public async Task<IActionResult> GetActiveCoursesBySpecialty(int specialtyId)
        {
            var courses = await _courseService.GetActiveCoursesBySpecialtyAsync(specialtyId);
            return Json(courses.Select(c => new { c.CourseID, c.Title }));
        }

        [HttpPost("RegisterCourse")]
        public async Task<IActionResult> RegisterCourse(int userId, int courseId, decimal scheduledHours)
        {
            try
            {
                var success = await _courseEnrollmentService.AddRegisteredEnrollmentAsync(userId, courseId, scheduledHours, 1, "Registered");
                if (!success)
                {
                    TempData["ErrorMessage1"] = "Enrollment failed.";
                }
                else
                {
                    TempData["SuccessMessage1"] = "Child enrolled successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage1"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("ManageRegistrations", new { userId });
        }


        [HttpPost("UnregisterCourse")]
        public async Task<IActionResult> UnregisterCourse(int enrollmentId, int userId)
        {
            try
            {
                var success = await _courseEnrollmentService.RemoveRegisteredEnrollmentAsync(enrollmentId);

                if (!success)
                {
                    TempData["ErrorMessage1"] = "Failed to remove enrollment.";
                }
                else
                {
                    TempData["SuccessMessage1"] = "Enrollment removed successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage1"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("ManageRegistrations", new { userId });
        }



        [HttpPost("RegisterActivity")]
        public async Task<IActionResult> RegisterActivity(int userId, int activityId)
        {
            try
            {
                var success = await _activityEnrollmentService.AddRegisteredEnrollmentAsync(userId, activityId, "Registered");

                if (!success)
                {
                    TempData["ErrorMessage2"] = "Failed to enroll in activity.";
                }
                else
                {
                    TempData["SuccessMessage2"] = "Successfully enrolled in activity.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage2"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("ManageRegistrations", new { userId });
        }

        [HttpPost("UnregisterActivity")]
        public async Task<IActionResult> UnregisterActivity(int enrollmentId, int userId)
        {
            try
            {
                var success = await _activityEnrollmentService.RemoveRegisteredEnrollmentAsync(enrollmentId);

                if (!success)
                {
                    TempData["ErrorMessage2"] = "Failed to remove enrollment.";
                }
                else
                {
                    TempData["SuccessMessage2"] = "Enrollment removed successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage2"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("ManageRegistrations", new { userId });
        }



        [HttpGet("ManagePayments/{childId}")]
        public async Task<IActionResult> ManagePayments(int childId)
        {
            // ✅ Pass ChildID to View
            //ViewBag.ChildID = childId;


            var payments = await _paymentService.GetByChildAsync(childId);
            var child = await _childService.GetChildByIdAsync(childId);
            if (child == null)
            {
                TempData["ErrorMessage"] = "Child not found.";
                return RedirectToAction("List"); // Redirect to child list page if not found
            }
            ManagePaymentsViewModel payment = new ManagePaymentsViewModel
            {
                Payments = payments,
                Child = child
            };

            var parents = await _paymentService.GetParentsByChildAsync(childId);

            // ✅ Populate Parent dropdown
            ViewBag.ParentList = parents.Select(p => new SelectListItem
            {
                Value = p.ParentID.ToString(),
                Text = p.Name
            }).ToList();

            // ✅ Fetch all active payment packages
            var packages = await _paymentService.GetAllActivePackagesAsync();

            // ✅ Populate ViewBag for dropdown
            ViewBag.PaymentPackages = packages.Select(p => new SelectListItem
            {
                Value = p.PackageID.ToString(),
                Text = p.Title
            }).ToList();

            return View("ManagePayments", payment);
        }


        [HttpPost("AddPayment")]
        
        public async Task<IActionResult> AddPayment(int childId, int parentId, int packageId, decimal amount, DateTime? paymentDate, IFormFile receiptFile)
        {

            try
            {
                string receiptPath = null;

                // ✅ Save the receipt file
                if (receiptFile != null && receiptFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/receipts");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = $"{Guid.NewGuid()}_{receiptFile.FileName}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await receiptFile.CopyToAsync(fileStream);
                    }

                    receiptPath = $"/receipts/{uniqueFileName}";
                }

                var result = await _paymentService.AddAsync(childId, parentId, packageId, amount, paymentDate, receiptPath);
                if (result)
                {
                    TempData["SuccessMessage"] = "Payment info has been added successfully.";
                    //return RedirectToAction("ManagePayments");
                    return RedirectToAction("ManagePayments", new { childId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Payment info was not added.";
                    return RedirectToAction("ManagePayments", new { childId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("ManagePayments", new { childId });
            }
        }


        [HttpPost("RemovePayment")]
        public async Task<IActionResult> RemovePayment(int paymentID, int childId)
        {
            try
            {
                var result = await _paymentService.RemoveAsync(paymentID);
                if (result)
                {
                    TempData["SuccessMessage"] = "Payment info has been deleted.";
                    return RedirectToAction("ManagePayments", new { childId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Payment info is not deleted.";
                    return RedirectToAction("ManagePayments", new { childId });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("ManagePayments", new { childId });
            }
            
            
        }


        [HttpGet("EnrollmentsHistory/{userId}")]

        public async Task<IActionResult> EnrollmentsHistory(int userId)
        {

            var child = await _childService.GetAsync(userId);
            var completedCourses = await _courseEnrollmentService.GetCompletedEnrollmentsByChildAsync(userId);
            var completedActivities = await _activityEnrollmentService.GetCompletedEnrollmentsByChildAsync(userId);

            if (child == null)
                throw new Exception("The child can't be found");

            EnrollmentsHistoryViewModel enrollmentHistory = new EnrollmentsHistoryViewModel
            {
                Child = child,
                CompletedCourses = (List<CourseEnrollment>) completedCourses,
                CompletedActivities = (List<ActivityEnrollment>)completedActivities
            };

            return View("EnrollmentsHistory", enrollmentHistory);
        }
        //[HttpGet("EditPayment/{paymentId}")]
        //public async Task<IActionResult> EditPayment(int paymentId)
        //{


        //    // ✅ Fetch Payment Details
        //    var payment = await _paymentService.GetByIdAsync(paymentId);
        //    if (payment == null)
        //    {
        //        TempData["ErrorMessage"] = "Payment not found.";
        //        return RedirectToAction("List");
        //    }

        //    var child = await _childService.GetChildByIdAsync(payment.ChildID);
        //    // ✅ Pass child details to View
        //    ViewBag.ChildID = child.ChildID;
        //    ViewBag.ChildName = child.Name;

        //    // ✅ Fetch Parents linked to the Child from ParentChild table
        //    var parents = await _paymentService.GetParentsByChildAsync(payment.ChildID);

        //    // ✅ Populate Parent dropdown
        //    ViewBag.ParentList = parents.Select(p => new SelectListItem
        //    {
        //        Value = p.ParentID.ToString(),
        //        Text = p.Name,
        //        Selected = (payment.ParentID == p.ParentID)
        //    }).ToList();

        //    // ✅ Fetch all active payment packages
        //    var packages = await _paymentService.GetAllActivePackagesAsync();

        //    // ✅ Populate ViewBag for dropdown
        //    ViewBag.PaymentPackages = packages.Select(p => new SelectListItem
        //    {
        //        Value = p.PackageID.ToString(),
        //        Text = p.Title,
        //        Selected = (payment.PaymentPackageID.HasValue && payment.PaymentPackageID.Value == p.PackageID)
        //    }).ToList();

        //    return View("Edit", payment);
        //}


    }



}

