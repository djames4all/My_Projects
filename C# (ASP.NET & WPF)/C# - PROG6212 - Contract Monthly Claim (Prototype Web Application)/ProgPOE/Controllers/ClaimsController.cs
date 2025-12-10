using Microsoft.AspNetCore.Mvc;
using ProgPOE.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using ProgPOE.Data;

namespace ProgPOE.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ClaimsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClaimModel claim, IFormFile uploadedFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (uploadedFile != null && uploadedFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(uploadedFile.FileName);
                        var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "UploadedFiles", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(stream);
                        }
                        claim.FilePath = fileName;
                    }

                    claim.Status = "Pending";
                    _context.Claims.Add(claim);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(claim);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while submitting your claim. Please try again.");
                return View(claim);
            }
        }

        public IActionResult Index()
        {
            var claims = _context.Claims.ToList();
            return View(claims);
        }

        public IActionResult Verify()
        {
            var claims = _context.Claims.Where(c => c.Status == "Pending").ToList();
            return View(claims);
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var claim = _context.Claims.Find(id);
            if (claim != null)
            {
                claim.Status = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Verify));
        }

        [HttpPost]
        public IActionResult Reject(int id)
        {
            var claim = _context.Claims.Find(id);
            if (claim != null)
            {
                claim.Status = "Rejected";
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Verify));
        }
    }
}
