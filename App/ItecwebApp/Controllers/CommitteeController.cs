using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItecwebApp.Controllers
{
    [Authorize]
    public class CommitteeController : Controller
    {
        private readonly ICommiteesDAl _committeeDAl;
        public CommitteeController(ICommiteesDAl committeeDAl)
        {
            _committeeDAl = committeeDAl;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Commitees c)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CommitteeModalBody", c);
            }

            var result = _committeeDAl.AddCommitee(c);
            if (result)
            {
                return Content("<div id='SuccessMessage'>Committee added successfully!</div>");
            }

            ModelState.AddModelError("", "Failed to add committee. Please try again.");
            return PartialView("_CommitteeModalBody", c);
        }

    }
}
