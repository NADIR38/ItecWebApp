using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItecwebApp.Controllers
{
    [Authorize]
    public class DutiesController : Controller
    {
        private readonly IDutiesDAl idl;
        public DutiesController(IDutiesDAl idl)
        {
            this.idl = idl;
        }

        public IActionResult Index()
        {
            var list = idl.getduties();
            return View(list);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Create(string name, string committeename)
        {
            var model = new Duties
            {
                name = name,
                committee_name = committeename
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin,Faculty")]
        public IActionResult Create(Duties d)
        {
            if (ModelState.IsValid)
            {
                if (idl.assign_duty(d))
                {
                    TempData["successMessage"] = "Duty assigned successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to assign duty. Please try again.");
                }
            }
            return View(d);
        }

        // ✅ Search API for AJAX
        public IActionResult Search(string search)
        {
            List<Duties> list;
            if (string.IsNullOrEmpty(search))
            {
                list = idl.getduties();
            }
            else
            {
                list = idl.search(search);
            }
            return Json(list);
        }

        // ✅ Edit (status update)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]

        public IActionResult Edit(Duties d)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = idl.Updatestatus(d);
                    if (result)
                    {
                        TempData["successMessage"] = "Status updated successfully!";
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to update status.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"An error occurred: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
