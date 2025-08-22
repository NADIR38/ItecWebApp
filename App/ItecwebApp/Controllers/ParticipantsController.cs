using ItecwebApp.DAL;
using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItecwebApp.Controllers
{
    public class ParticipantsController : Controller
    {
        private readonly IParticipantsDAL idl;
        public ParticipantsController(IParticipantsDAL idl)
        {
            this.idl = idl;
        }
        public IActionResult Index()
        {
            var participants = idl.GetAllParticipants();
            return View(participants);
        }
        [HttpGet]
        public IActionResult Add()
        {
            var list = DatabaseHelper.geteventnames("");
            ViewBag.EventNames = list ?? new List<string>(); // never null
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // add this for security
        public IActionResult Add(Models.Participants participant)
        {
            if (ModelState.IsValid)
            {
                if (idl.AddParticipant(participant))
                {
                    TempData["Message"] = "Participant added successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to add participant. Please try again.");
                }
            }

            // repopulate event names if returning to view
            ViewBag.EventNames = DatabaseHelper.geteventnames("");
            return View(participant);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var list = DatabaseHelper.geteventnames("");
            ViewBag.EventNames = list ?? new List<string>(); // never null
            var participant = idl.GetAllParticipants().FirstOrDefault(s => s.Id == id);
            if (participant == null)
            {
                TempData["ErrorMessage"] = "Participant not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(participant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Add this for consistency
        public IActionResult Edit(Models.Participants participant)
        {
            if (ModelState.IsValid)
            {
                if (idl.UpdateParticipant(participant))
                {
                    TempData["Message"] = "Participant updated successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update participant. Please try again.");
                }
            }

            // Repopulate event names if returning to view
            var list = DatabaseHelper.geteventnames("");
            ViewBag.EventNames = list ?? new List<string>();
            return View(participant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var success = idl.DeleteParticipant(id);
            if (success)
            {
                TempData["Message"] = "Participant deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete participant. Please try again.";
            }
            return RedirectToAction("Index");


        }
        public IActionResult Search(string searchTerm)
        {
            List<Participants> list;
            if(string.IsNullOrEmpty(searchTerm))
            {
                list = idl.GetAllParticipants();
            }
            else
            {
                list = idl.SearchParticipants(searchTerm);
            }
            return Json(list);

        }
    }
}
