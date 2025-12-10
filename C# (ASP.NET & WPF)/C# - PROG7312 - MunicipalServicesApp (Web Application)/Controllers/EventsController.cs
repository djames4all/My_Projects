using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;
using System;
using System.Linq;

namespace MunicipalServicesApp.Controllers
{
    public class EventsController : Controller
    {
        // GET: /Events
        public IActionResult Index(string? category, DateTime? from, DateTime? to, string? keyword)
        {
            ViewData["Title"] = "Local Events & Announcements";

            ViewBag.Categories = EventRepository.GetCategories();
            ViewBag.Upcoming = EventRepository.GetUpcoming(10);

            var results = EventRepository.Search(category, from, to, keyword).ToList();
            ViewBag.Results = results;

            ViewBag.Recommendations = EventRepository.Recommend(string.IsNullOrWhiteSpace(category) ? null : category, 5);
            ViewBag.RecentlyViewed = EventRepository.GetRecentlyViewed(5);

            ViewBag.SelectedCategory = category ?? "";
            ViewBag.Keyword = keyword ?? "";
            ViewBag.From = from?.ToString("yyyy-MM-dd") ?? "";
            ViewBag.To = to?.ToString("yyyy-MM-dd") ?? "";

            return View();
        }

        // GET: /Events/Details/5
        public IActionResult Details(int id)
        {
            var ev = EventRepository.GetById(id);
            if (ev == null) return NotFound();

            EventRepository.MarkViewed(id);

            ViewBag.Recommendations = EventRepository.Recommend(ev.Category, 5);
            ViewBag.RecentlyViewed = EventRepository.GetRecentlyViewed(5);

            return View(ev);
        }

        // GET: /Events/Create
        public IActionResult Create()
        {
            ViewBag.Categories = EventRepository.GetCategories();
            return View();
        }

        // POST: /Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid event data.";
                return RedirectToAction(nameof(Create));
            }

            EventRepository.AddEvent(model);
            TempData["Message"] = "Event added successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
