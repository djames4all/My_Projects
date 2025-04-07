using Microsoft.AspNetCore.Mvc;
using ProgPOE.Data;
using ProgPOE.Models;
using System.Linq;

namespace ProgPOE.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LecturerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch claim data from the database
            var claims = _context.Claims.ToList();
            return View(claims);
        }

        [HttpPost]
        public IActionResult SubmitClaim(ClaimModel claim)
        {
            if (ModelState.IsValid)
            {
                _context.Claims.Add(claim);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpGet]
        public IActionResult ViewClaim(int id)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null) return NotFound();

            return View(claim);
        }
    }
}
