
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
        private readonly AppDbContext _context;

        public CityController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Load City List
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var cities = await _context.Cities.ToListAsync();
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
            

            var city = await _context.Cities.FindAsync(cityId);
            if (city == null) return NotFound();

            return PartialView("_Edit", city);
        }

        //[HttpGet("AddEdit/{cityId}")]
        //public async Task<IActionResult> AddEdit(int? cityId)
        //{
        //    if (cityId == 0) return PartialView("_AddEdit", new City { Name = string.Empty });

        //    var city = await _context.Cities.FindAsync(cityId);
        //    if (city == null) return NotFound();

        //    return PartialView("_AddEdit", city);
        //}



        // ✅ Save City (Add / Edit)
        [HttpPost("Save")]
        public async Task<IActionResult> Save(City city)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid data");
                city.CreatedBy = 1;  //temparaly set it to 1

                if (city.CityID == 0)
                    _context.Cities.Add(city); // Add new city
                else
                    _context.Cities.Update(city); // Edit existing city

                await _context.SaveChangesAsync();
                return Json(new { success = true });
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

            var city = await _context.Cities.FindAsync(cityId);
            if (city == null) return NotFound();

            return PartialView("_DeleteConfirm", city);
        }

        // ✅ Delete City
        [HttpPost("Delete/{cityId}")]
        public async Task<IActionResult> Delete(int cityId)
        {
            var city = await _context.Cities.FindAsync(cityId);
            if (city == null) return NotFound();

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }

}
