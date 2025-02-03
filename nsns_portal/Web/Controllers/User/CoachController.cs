
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
    [Route("Coach")]
    //[ApiController]
    public class CoachController : Controller
    {
        private readonly ICoachService _coachService;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Specialty> _specialtyRepository;

        public CoachController(ICoachService coachService, IRepository<City> cityRepository, IRepository<Specialty> specialtyRepository)
        {
            _coachService = coachService;
            _cityRepository = cityRepository;
            _specialtyRepository = specialtyRepository;
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

                    var cities = await _cityRepository.GetAllAsync(); // Replace with your data fetching logic
                    ViewBag.CityList = cities.Select(c => new SelectListItem
                    {
                        Value = c.CityID.ToString(),
                        Text = c.Name
                    }).ToList();

                    var specialties = await _specialtyRepository.GetAllAsync(); // Replace with your data fetching logic
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

                var cities = await _cityRepository.GetAllAsync(); // Replace with your data fetching logic
                ViewBag.CityList = cities.Select(c => new SelectListItem
                {
                    Value = c.CityID.ToString(),
                    Text = c.Name
                }).ToList();

                var specialties = await _specialtyRepository.GetAllAsync(); // Replace with your data fetching logic
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
            var cities = await _cityRepository.GetAllAsync(); // Replace with your data fetching logic
            ViewBag.CityList = cities.Select(c => new SelectListItem
            {
                Value = c.CityID.ToString(),
                Text = c.Name
            }).ToList();

            var specialties = await _specialtyRepository.GetAllAsync(); // Replace with your data fetching logic
            ViewBag.SpecialtyList = specialties.Select(c => new SelectListItem
            {
                Value = c.SpecialtyID.ToString(),
                Text = c.Title
            }).ToList();


            return View();

        }


       




        // GET: Staff/Delete/{userId}
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

            var cities = await _cityRepository.GetAllAsync(); // Replace with your data fetching logic



            ViewBag.CityList = cities.Select(c => new SelectListItem
            {
                Value = c.CityID.ToString(),
                Text = c.Name,
                Selected = c.CityID == coach.CityID
            }).ToList();


            var specialties = await _specialtyRepository.GetAllAsync(); // Replace with your data fetching logic

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

                    var cities = await _cityRepository.GetAllAsync(); // Replace with your data fetching logic



                    ViewBag.CityList = cities.Select(c => new SelectListItem
                    {
                        Value = c.CityID.ToString(),
                        Text = c.Name,
                        Selected = c.CityID == coach.CityID
                    }).ToList();


                    var specialties = await _specialtyRepository.GetAllAsync(); // Replace with your data fetching logic

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
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                var coach = await _coachService.GetAsync(userId);

                if (coach == null)
                {
                    return NotFound();
                }

                var cities = await _cityRepository.GetAllAsync(); // Replace with your data fetching logic



                ViewBag.CityList = cities.Select(c => new SelectListItem
                {
                    Value = c.CityID.ToString(),
                    Text = c.Name,
                    Selected = c.CityID == coach.CityID
                }).ToList();


                var specialties = await _specialtyRepository.GetAllAsync(); // Replace with your data fetching logic

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




    }
}

