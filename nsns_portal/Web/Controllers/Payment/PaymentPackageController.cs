﻿
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
    [Route("PaymentPackage")]
    public class PaymentPackageController: Controller
    {
        private readonly IPaymentPackageService _paymentPackageService;

        public PaymentPackageController(IPaymentPackageService paymentPackageService)
        {
            _paymentPackageService = paymentPackageService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var packages = await _paymentPackageService.GetAllAsync();
            return View(packages);
        }

        [HttpGet("AddEdit/{packageId?}")]
        public async Task<IActionResult> AddEdit(int? packageId)
        {
            var package = packageId.HasValue ? await _paymentPackageService.GetByIdAsync(packageId.Value) : new PaymentPackage();
            return View("AddEdit", package);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(PaymentPackage package)
        {
            if (!ModelState.IsValid) return View("AddEdit", package);
            try
            {
                if (package.PackageID == 0)
                {
                    var result = await _paymentPackageService.AddAsync(package);
                    if(result)
                        TempData["SuccessMessage"] = "Payment Package info has been added successfully.";
                    else
                        TempData["ErrorMessage"] = "Payment Package info is not being added.";
                }
                else
                {
                    var result = await _paymentPackageService.UpdateAsync(package);
                    if(result)
                        TempData["SuccessMessage"] = "Payment Package info has been updated successfully.";
                    else
                        TempData["ErrorMessage"] = "Payment Package info is not being updated.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("List");
        }

        [HttpGet("ConfirmDelete/{packageId}")]
        public async Task<IActionResult> ConfirmDelete(int packageId)
        {
            var package = await _paymentPackageService.GetByIdAsync(packageId);
            return View("ConfirmDelete", package);
        }

        [HttpPost("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int packageId)
        {
            try
            {
                var result = await _paymentPackageService.RemoveAsync(packageId);
                if (result)
                {
                    TempData["SuccessMessage"] = "Payment Package info has been deleted successfully.";
                    return RedirectToAction("List");
                }
                else
                {
                    TempData["ErrorMessage"] = "Payment Package info can't be deleted.";
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("List");
            }
        }
    }
}
