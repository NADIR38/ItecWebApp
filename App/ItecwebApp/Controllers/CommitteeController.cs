using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItecwebApp.Controllers
{
    public class CommitteeController : Controller
    {
        private readonly ICommiteesDAl _committeeDAl;
        public CommitteeController(ICommiteesDAl committeeDAl)
        {
            _committeeDAl = committeeDAl;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Commitees c)
        {
            if (!ModelState.IsValid)
            {
                // Return partial view of the modal content with validation messages
                return PartialView("_CommitteeModalBody", c);
            }

            var result = _committeeDAl.AddCommitee(c);
            if (result)
            {
                // Return a small message or flag indicating success
                return Content("<div id='SuccessMessage'>Committee added successfully!</div>");
            }

            ModelState.AddModelError("", "Failed to add committee. Please try again.");
            return PartialView("_CommitteeModalBody", c);
        }

    }
}
