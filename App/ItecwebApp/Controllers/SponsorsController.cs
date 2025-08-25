using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design.Serialization;

namespace ItecwebApp.Controllers
{
    [Authorize]

    public class SponsorsController : Controller
    {
        private readonly ISponsorsDAl _sponsorsDal;
        public SponsorsController(ISponsorsDAl sponsorsDal)
        {
            _sponsorsDal = sponsorsDal;
        }

        public IActionResult Index()
        {
            var list=_sponsorsDal.GetSponsors();
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sponsors s)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Invalid input. Please try again.";
                    return RedirectToAction(nameof(Index));
                }

                var result = _sponsorsDal.AddSponsor(s);

                if (result)
                {
                    TempData["SuccessMessage"] = "Sponsor added successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to add sponsor.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while saving sponsor.";
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var sponsor = _sponsorsDal.GetSponsors().FirstOrDefault(s => s.SponsorId == id);
            if (sponsor == null)
            {
                TempData["ErrorMessage"] = "Sponsor not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(sponsor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Sponsors s)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Invalid input. Please try again.";
                    return RedirectToAction(nameof(Index));
                }
                var result = _sponsorsDal.UpdateSponsor(s);
                if (result)
                {
                    TempData["SuccessMessage"] = "Sponsor updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to update sponsor.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while updating sponsor.";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Search(string SearchTerm)
        {
            List<Sponsors> list;
            if (string.IsNullOrEmpty(SearchTerm))
            {
                list=_sponsorsDal.GetSponsors();
            }
            else
            {
                list = _sponsorsDal.searchsponsors(SearchTerm);
            }
            return Json(list);
        }

        }
}
