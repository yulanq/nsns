﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Income
{
    public class IncomeController : Controller
    {
        // GET: IncomeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: IncomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: IncomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IncomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: IncomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: IncomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: IncomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IncomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
