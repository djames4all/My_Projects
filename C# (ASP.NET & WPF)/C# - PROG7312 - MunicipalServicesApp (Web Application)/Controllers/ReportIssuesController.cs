using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Controllers
{
    public class ReportIssuesController : Controller
    {
        // In-memory storage
        private static readonly List<ReportIssue> _issues = new();

        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ReportIssuesController> _logger;

        public ReportIssuesController(IWebHostEnvironment env, ILogger<ReportIssuesController> logger)
        {
            _env = env;
            _logger = logger;
        }

        // GET: /ReportIssues/Create
        public IActionResult Create()
        {
            // Pass all submitted issues to the view for the second tab
            ViewBag.Reports = _issues.OrderByDescending(i => i.SubmittedAt).ToList();
            return View();
        }

        // POST: /ReportIssues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReportIssue model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Reports = _issues.OrderByDescending(i => i.SubmittedAt).ToList();
                return View(model);
            }

            // Validate and save file (if any)
            if (model.MediaFile != null && model.MediaFile.Length > 0)
            {
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
                var extension = Path.GetExtension(model.MediaFile.FileName).ToLowerInvariant();

                // Error handling for unsupported file types
                if (!allowed.Contains(extension))
                {
                    ModelState.AddModelError("MediaFile", "Invalid file type. Only images and PDFs allowed.");
                    ViewBag.Reports = _issues.OrderByDescending(i => i.SubmittedAt).ToList();
                    return View(model);
                }

                // Error handling for files too large
                const long maxSize = 5 * 1024 * 1024;
                if (model.MediaFile.Length > maxSize)
                {
                    ModelState.AddModelError("MediaFile", "File exceeds 5 MB limit.");
                    ViewBag.Reports = _issues.OrderByDescending(i => i.SubmittedAt).ToList();
                    return View(model);
                }

                // Save valid file
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var uniqueName = $"{Guid.NewGuid()}{extension}";
                var fullPath = Path.Combine(uploads, uniqueName);

                using (var fs = new FileStream(fullPath, FileMode.Create))
                {
                    model.MediaFile.CopyTo(fs);
                }

                model.MediaFilePath = $"/uploads/{uniqueName}";
            }


            // Assign ID and submitted time
            model.Id = (_issues.Count > 0) ? _issues.Max(i => i.Id) + 1 : 1;
            model.SubmittedAt = DateTime.Now;
            model.Status = "Pending";

            _issues.Add(model);

            // Provide success feedback but stay on form so user can submit more issues
            ViewBag.SuccessMessage = "Thank you — your issue was submitted successfully!";
            ModelState.Clear();

            // Pass updated list to the view for second tab
            ViewBag.Reports = _issues.OrderByDescending(i => i.SubmittedAt).ToList();
            return View();
        }

        // GET: /ReportIssues (list all issues)
        public IActionResult Index()
        {
            var ordered = _issues.OrderByDescending(i => i.SubmittedAt).ToList();
            return View(ordered);
        }

        // POST (AJAX): update status quickly
        [HttpPost]
        public IActionResult UpdateStatusAjax(int id, string status)
        {
            var issue = _issues.FirstOrDefault(i => i.Id == id);
            if (issue == null) return Json(new { success = false, message = "Issue not found." });

            var allowed = new[] { "Pending", "In Progress", "Resolved" };
            if (!allowed.Contains(status)) return Json(new { success = false, message = "Invalid status." });

            issue.Status = status;
            return Json(new { success = true, newStatus = status });
        }
    }
}
