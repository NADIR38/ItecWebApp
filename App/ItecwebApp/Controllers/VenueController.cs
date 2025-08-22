using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItecwebApp.Controllers
{
    public class VenueController : Controller
    {
        private readonly IVenueDAl idl;
        public VenueController(IVenueDAl idl)
        {
            this.idl = idl;
        }
        public IActionResult Index()
        {
            var list = idl.GetVenues();
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Create(Venues v)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if (idl.addvenue(v))
                    {
                        TempData["SuccessMessage"] = "Venue added successfully!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to add venue. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }
            return View(v);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var venue = idl.GetVenues().FirstOrDefault(v => v.id == id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
        [HttpPost]
        public IActionResult Edit(Venues v)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (idl.EditVenue(v))
                    {
                        TempData["SuccessMessage"] = "Venue updated successfully!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update venue. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }
            return View(v);
        }
        [HttpGet]
        public IActionResult Search(string term)
        {
            List<Venues> list;
            if (string.IsNullOrEmpty(term))
            {
                list = idl.GetVenues();
            }
            else
            {
                list = idl.searchvenues(term);
            }
           return Json(list);

        }


    }
}
