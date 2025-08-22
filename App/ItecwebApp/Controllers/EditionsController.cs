using ItecwebApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ItecwebApp.Controllers
{
    public class EditionsController : Controller
    {
        private readonly IEditionDAl _editionDAl;
        public EditionsController(IEditionDAl editionDAl)
        {
            _editionDAl = editionDAl;
        }
        public IActionResult Index()
        {
            var list = _editionDAl.GetEditions();

            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Models.Edition edition)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_editionDAl.AddEdition(edition))
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Failed to add edition. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while adding the edition: {ex.Message}");
            }
            return View(edition);

        }
    }
}