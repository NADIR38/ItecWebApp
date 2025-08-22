using ItecwebApp.Interfaces;
using ItecwebApp.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace ItecwebApp.Controllers
{
    public class MembersController : Controller
    {
        private readonly ICommiteeMembersDal idl;
        public MembersController(ICommiteeMembersDal idl)
        {
            this.idl = idl;
        }
        public IActionResult Index()
        {
            var list = idl.getallmembers();
            return View(list);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CommitteMemeber m)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (idl.AddMembers(m))
                    {
                        TempData["Successmessage"] = "Member added successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Errormessage"] = "Failed to add member. Please try again.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Errormessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Search(string searchTerm)
        {
            List<CommitteMemeber> list;
            if (string.IsNullOrEmpty(searchTerm))
            {
                list=idl.getallmembers();
            }
            else
            {
                list = idl.searchmembers(searchTerm);
            }
            return Json(list);
        }
        public IActionResult SearchCommittees(string term)
        {
            var commitees = DatabaseHelper.getcommittenames(term) ?? new List<string>();
            return Json(commitees.Select(c => new { name = c }));
        }

    }

}
