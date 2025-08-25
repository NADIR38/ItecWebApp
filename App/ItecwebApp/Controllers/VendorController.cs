using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItecwebApp.Controllers
{
    [Authorize(Roles = "Admin")]

    public class VendorController : Controller
    {
        private readonly IVendorsDAl idl;
        public VendorController(IVendorsDAl idl)
        {
            this.idl = idl;
        }
        public IActionResult Index()
        {
            var list = idl.GetVendors();
            return View(list);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Vendors v)
        {
            try { 
            if (ModelState.IsValid)
            {

                    if (idl.AddVendor(v))
                    {
                        TempData["SuccessMessage"] = "Vendor added successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to add vendor. Please try again.";
                        return RedirectToAction(nameof(Index));
                    }

                }
            }


            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Vendors v)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (idl.EditVendor(v))
                    {
                        TempData["SuccessMessage"] = "Vendor updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to update vendor. Please try again.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Search(string search)
        {
            List<Vendors> list;
            if(string.IsNullOrEmpty(search))
            {
                list=idl.GetVendors();
            }
            else
            {
                list=idl.Search(search);
            }
            return Json(list);
        }

    }
}
