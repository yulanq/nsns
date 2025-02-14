
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
using Microsoft.AspNetCore.Http.HttpResults;
using Core.ViewModels;



namespace Web.Controllers.Payment
{
    [Route("Payment")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IChildService _childService;

        public PaymentController(IPaymentService paymentService, IChildService childService)
        {
            _paymentService = paymentService;
            _childService = childService;
        }

        //[HttpGet("List/{childId}")]
        //public async Task<IActionResult> List(int childId)
        //{
        //    // ✅ Pass ChildID to View
        //    //ViewBag.ChildID = childId;
           

        //    var payments = await _paymentService.GetByChildAsync(childId);
        //    var child = await _childService.GetChildByIdAsync(childId);
        //    if (child == null)
        //    {
        //        TempData["ErrorMessage"] = "Child not found.";
        //        return RedirectToAction("List"); // Redirect to child list page if not found
        //    }
        //    ManagePaymentsViewModel payment = new ManagePaymentsViewModel
        //    {
        //        Payments = payments,
        //        Child = child
        //    };
        //    return View("List", payment);
        //}



        //[HttpGet("Add/{childId}")]
        //public async Task<IActionResult> Add(int childId)
        //{
        //    // var payment = new Core.Models.Payment();


        //    // ✅ Fetch Child Details
        //    var child = await _childService.GetChildByIdAsync(childId);
        //    if (child == null)
        //    {
        //        TempData["ErrorMessage"] = "Child not found.";
        //        return RedirectToAction("List"); // Redirect if child doesn't exist
        //    }

        //    // ✅ Pass child details to View
        //    ViewBag.ChildID = child.ChildID;
        //    ViewBag.ChildName = child.Name;
        //    //ViewBag.CreatedBy = 1;

        //    // ✅ Fetch Parents linked to the Child from ParentChild table
        //    var parents = await _paymentService.GetParentsByChildAsync(childId);

        //    // ✅ Populate Parent dropdown
        //    ViewBag.ParentList = parents.Select(p => new SelectListItem
        //    {
        //        Value = p.ParentID.ToString(),
        //        Text = p.Name
        //    }).ToList();

        //    // ✅ Fetch all active payment packages
        //    var packages = await _paymentService.GetAllActivePackagesAsync();

        //    // ✅ Populate ViewBag for dropdown
        //    ViewBag.PaymentPackages = packages.Select(p => new SelectListItem
        //    {
        //        Value = p.PackageID.ToString(),
        //        Text = p.Title
        //    }).ToList();

        //    return View("Add");
        //}



        //[HttpGet("Edit/{paymentId}")]
        //public async Task<IActionResult> Edit(int paymentId)
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

        //[HttpPost("Add")]
        ////public async Task<IActionResult> Add(Core.Models.Payment payment)
        //public async Task<IActionResult> Add(int childId, int parentId, int packageId, decimal amount)
        //{
           
        //    try
        //    {
        //        var result = await _paymentService.AddAsync( childId,  parentId, packageId,  amount);
        //        if (result)
        //        {
        //            TempData["SuccessMessage"] = "Payment info has been added successfully.";
        //            return RedirectToAction("List");
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Payment info was not added.";
        //            return RedirectToAction("List");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = $"Error: {ex.Message}";
        //        return RedirectToAction("List");
        //    }
            
         

            
        //}

        //[HttpPost("Edit")]
        //public async Task<IActionResult> Edit(Core.Models.Payment payment)
        //{
        //    if (!ModelState.IsValid) return View("Edit", payment);

           
        //     await _paymentService.UpdateAsync(payment);

        //    return RedirectToAction("List");
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeleteConfirmed(int paymentID)
        //{
        //    await _paymentService.RemoveAsync(paymentID);
        //    return RedirectToAction("List");
        //}
    }
}
