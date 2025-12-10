using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Controllers
{
    public class CommunityController : Controller
    {
        private static readonly List<SurveyResponse> _surveys = new();
        private static readonly List<Suggestion> _suggestions = new();

        // GET: /Community/Engagement
        public IActionResult Engagement()
        {
            ViewBag.Surveys = _surveys.OrderByDescending(s => s.SubmittedAt).ToList();
            ViewBag.Suggestions = _suggestions.OrderByDescending(s => s.SubmittedAt).ToList();
            return View();
        }

        // POST: /Community/SubmitSurvey
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitSurvey(SurveyResponse response)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Surveys = _surveys;
                ViewBag.Suggestions = _suggestions;
                return View("Engagement", response);
            }

            response.SubmittedAt = DateTime.Now;
            response.Id = _surveys.Count > 0 ? _surveys.Max(s => s.Id) + 1 : 1;
            _surveys.Add(response);

            TempData["Message"] = "✅ Thank you for completing the survey!";
            return RedirectToAction("Engagement");
        }

        // POST: /Community/SubmitSuggestion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitSuggestion(Suggestion suggestion)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Surveys = _surveys;
                ViewBag.Suggestions = _suggestions;
                return View("Engagement", suggestion);
            }

            suggestion.SubmittedAt = DateTime.Now;
            suggestion.Id = _suggestions.Count > 0 ? _suggestions.Max(s => s.Id) + 1 : 1;
            _suggestions.Add(suggestion);

            TempData["Message"] = "💡 Thank you for your suggestion!";
            return RedirectToAction("Engagement");
        }
    }
}
