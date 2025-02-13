
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



namespace Web.Controllers.Payment
{
    [Route("Payment")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var payments = await _paymentService.GetAllAsync();
            return View("List", payments);
        }



        [HttpGet("AddEdit/{paymentId?}")]
        public async Task<IActionResult> AddEdit(int childId, int? paymentId)
        {
            var payment = paymentId.HasValue ? await _paymentService.GetByIdAsync(paymentId.Value) : new Core.Models.Payment();

            
            // ✅ Fetch Child Details
            var child = await _paymentService.GetChildByIdAsync(childId);
            if (child == null)
            {
                TempData["ErrorMessage"] = "Child not found.";
                return RedirectToAction("List"); // Redirect if child doesn't exist
            }

            // ✅ Pass child details to View
            ViewBag.ChildID = child.ChildID;
            ViewBag.ChildName = child.Name;

            // ✅ Fetch Parents linked to the Child from ParentChild table
            var parents = await _paymentService.GetParentsByChildAsync(childId);

            // ✅ Populate Parent dropdown
            ViewBag.ParentList = parents.Select(p => new SelectListItem
            {
                Value = p.ParentID.ToString(),
                Text = p.Name,
                Selected = (payment.ParentID.HasValue && payment.ParentID.Value == p.ParentID)
            }).ToList();

            // ✅ Fetch all active payment packages
            var packages = await _paymentService.GetAllActivePackagesAsync();

            // ✅ Populate ViewBag for dropdown
            ViewBag.PaymentPackages = packages.Select(p => new SelectListItem
            {
                Value = p.PackageID.ToString(),
                Text = p.Title,
                Selected = (payment.PaymentPackageID.HasValue && payment.PaymentPackageID.Value == p.PackageID)
            }).ToList();

            return View("AddEdit", payment);
        }


        [HttpGet("GetParentsByChild")]
        public async Task<IActionResult> GetParentsByChild(int childId)
        {
            var parents = await _paymentService.GetParentsByChildAsync(childId);
            var parentList = parents.Select(p => new
            {
                parentID = p.ParentID,
                name = p.Name
            }).ToList();

            return Json(parentList);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(Core.Models.Payment payment)
        {
            if (!ModelState.IsValid) return View("AddEdit", payment);

            if (payment.PaymentID == 0)
                await _paymentService.AddAsync(payment);
            else
                await _paymentService.UpdateAsync(payment);

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int paymentID)
        {
            await _paymentService.RemoveAsync(paymentID);
            return RedirectToAction("List");
        }
    }
}
