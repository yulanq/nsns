
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

        public ChildController(IChildService childService, IParentService parentService, ICityService cityService, IParentChildService parentChildService)
        {
            _childService = childService;
            _parentService = parentService;
            _cityService = cityService;
            _parentChildService = parentChildService;
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



        [HttpGet("ManageParents")]
        public async Task<IActionResult> ManageParents(int childId)
        {
            var child = await _childService.GetChildByIdAsync(childId);
            if (child == null)
            {
                TempData["ErrorMessage"] = "Child not found.";
                return RedirectToAction("List");
            }

            var parents = await _parentChildService.GetParentsByChildIdAsync(childId);
            ViewBag.ParentList = (await _parentService.GetAllParentsAsync())
                .Select(p => new SelectListItem { Value = p.ParentID.ToString(), Text = p.Name })
                .ToList();

            var model = new ManageParentsViewModel
            {
                Child = child,
                Parents = parents
            };

            return View(model);
        }


        [HttpPost("AddParentToChild")]
        public async Task<IActionResult> AddParentToChild(int parentId, int childId, string relationship)
        {
            try
            {
                var success = await _parentChildService.AddParentToChild(parentId, childId, relationship, 1); // Assuming CreatedBy = 1
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


        [HttpPost]
        public async Task<IActionResult> RemoveParentFromChild(int parentChildId, int childId)
        {
            try
            {
                var success = await _parentChildService.RemoveParentFromChild(parentChildId);
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



    }


 
}

