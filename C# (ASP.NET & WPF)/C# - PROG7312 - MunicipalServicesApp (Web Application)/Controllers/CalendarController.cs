using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.Controllers
{
    public class CalendarController : Controller
    {
        private const string CalendarSessionKey = "UserCalendar";

        // GET: /Calendar
        public IActionResult Index()
        {
            var calendar = GetCalendarFromSession();
            return View(calendar);
        }

        // POST: /Calendar/AddToCalendar
        [HttpPost]
        public IActionResult AddToCalendar(EventModel eventModel)
        {
            var calendar = GetCalendarFromSession();

            if (!calendar.Any(e => e.Id == eventModel.Id))
            {
                calendar.Add(eventModel);
                SaveCalendarToSession(calendar);
            }

            TempData["Message"] = $"'{eventModel.Title}' has been added to your calendar.";
            return RedirectToAction("Index", "Calendar");
        }

        // POST: /Calendar/RemoveFromCalendar
        [HttpPost]
        public IActionResult RemoveFromCalendar(int id)
        {
            var calendar = GetCalendarFromSession();
            var item = calendar.FirstOrDefault(e => e.Id == id);

            if (item != null)
            {
                calendar.Remove(item);
                SaveCalendarToSession(calendar);
            }

            return RedirectToAction("Index");
        }

        // Helper Method: Retrieves the user's calendar data from session storage.
        private List<EventModel> GetCalendarFromSession()
        {
            var sessionData = HttpContext.Session.GetString(CalendarSessionKey);
            if (string.IsNullOrEmpty(sessionData)) return new List<EventModel>();
            return JsonConvert.DeserializeObject<List<EventModel>>(sessionData) ?? new List<EventModel>();
        }

        // Helper Method: Saves the user's updated calendar back into session.
        private void SaveCalendarToSession(List<EventModel> calendar)
        {
            var json = JsonConvert.SerializeObject(calendar);
            HttpContext.Session.SetString(CalendarSessionKey, json);
        }
    }
}
