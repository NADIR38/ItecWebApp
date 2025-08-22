using ItecwebApp.DAL;
using Microsoft.AspNetCore.Mvc;
using ItecwebApp.Models;

namespace ItecwebApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventsDAll idl;
        public EventsController(IEventsDAll idl)
        {
            this.idl = idl;
        }

        // Show all events
        public IActionResult Index()
        {
            var list = idl.GetEvents();
            return View(list);
        }

        // -------------------- CREATE --------------------
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Events e)
        {
            if (ModelState.IsValid)
            {
                if (idl.AddEvent(e))
                    return RedirectToAction("Index");
            }
            return View(e);
        }

        // -------------------- GET EVENT BY ID --------------------
        [HttpGet]
        public IActionResult GetEventById(int id)
        {
            var ev = idl.GetEvents().FirstOrDefault(x => x.event_id == id);
            if (ev == null) return NotFound();
            return Json(ev);
        }

        // -------------------- UPDATE EVENT --------------------
        [HttpPost]
        public IActionResult UpdateEvent([FromBody] Events e)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data");

            var result = idl.updatevent(e);
            if (result) return Ok();
            return BadRequest("Update failed");
        }

        // -------------------- DELETE EVENT --------------------
        [HttpPost]
        //public IActionResult Delete(int id)
        //{
        //    var result = idl.deletevent(id);
        //    if (result) return Ok();
        //    return BadRequest("Delete failed");
        //}
    



public IActionResult SearchVenues(string searchTerm)
        {
            var venues = DatabaseHelper.getvenuenames(searchTerm) ?? new List<string>();
            // Return JSON array of objects with name property
            return Json(venues.Select(v => new { name = v }));
        }

        public IActionResult SearchCommittees(string term)
        {
            var commitees = DatabaseHelper.getcommittenames(term) ?? new List<string>();
            return Json(commitees.Select(c => new { name = c }));
        }

        public IActionResult SearchCategories()
        {
            var categories = DatabaseHelper.getcategorynames() ?? new List<string>();
            return Json(categories.Select(c => new { name = c }));
        }
        public IActionResult SearchEvents(string searchTerm)
        {
            var events = idl.GetEvents(searchTerm ?? ""); // ✅ if null, use ""
            return Json(events.Select(e => new {
                e.event_id,
                e.event_name,
                e.event_description,
                e.event_date,
                e.venue_name,
                e.committee_name,
                e.year,
                e.category_name,
                e.assigned_time
            }));
        }

    }
}