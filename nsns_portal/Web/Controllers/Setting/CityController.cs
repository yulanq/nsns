
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Models;
using Core.Contexts;
using System.Diagnostics;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;



namespace Web.Controllers.Setting
{
    [Route("City")]
    public class CityController : Controller
    {
        //private readonly AppDbContext _context;
        private ICityService _cityService;
        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        // ✅ Load City List
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            //var cities = await _context.Cities.ToListAsync();
            var cities =await _cityService.GetAllAsync();
            return View(cities);
        }

        // ✅ Load Partial View for Add/Edit Form
        [HttpGet("Add")]
        public async Task<IActionResult> Add()
        {
            return PartialView("_Add", new City{ Name = string.Empty });
        }


        [HttpGet("Edit/{cityId}")]
        public async Task<IActionResult> Edit(int cityId)
        {


            //var city = await _context.Cities.FindAsync(cityId);
            var city = await _cityService.GetAsync(cityId);
            if (city == null) return NotFound();

            return PartialView("_Edit", city);
        }





        // ✅ Save City (Add / Edit)
        [HttpPost("Save")]
        public async Task<IActionResult> Save(City city)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid data");
                city.CreatedBy = 1;  //temparaly set it to 1
                var result = false;
                if (city.CityID == 0)
                    //_context.Cities.Add(city); // Add new city
                    result = await _cityService.AddAsync(city);
                else
                    //_context.Cities.Update(city); // Edit existing city
                    result = await _cityService.UpdateAsync(city);

                //await _context.SaveChangesAsync();
                if (result)
                    return Json(new { success = true });
                else
                    return Json(new { success = false });

            }
                
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
            
        }


        // ✅ Load Partial View for Add/Edit Form
        [HttpGet("DeleteConfirm/{cityId}")]
        public async Task<IActionResult> DeleteConfirm(int cityId)
        {
            //if (cityId == 0) return PartialView("_DeleteConfirm", new City { Name = string.Empty });

            //var city = await _context.Cities.FindAsync(cityId);
            var city = await _cityService.GetAsync(cityId);
            if (city == null) return NotFound();

            return PartialView("_DeleteConfirm", city);
        }

        // ✅ Delete City
        [HttpPost("Delete/{cityId}")]
        public async Task<IActionResult> Delete(int cityId)
        {
            //var city = await _context.Cities.FindAsync(cityId);
            var result = await _cityService.DeleteAsync(cityId);
            //if (city == null) return NotFound();

            //_context.Cities.Remove(city);
            //await _context.SaveChangesAsync();
            if (result)
                return Json(new { success = true });
            else
                return Json(new { success = false });
           
        }
    }

}
