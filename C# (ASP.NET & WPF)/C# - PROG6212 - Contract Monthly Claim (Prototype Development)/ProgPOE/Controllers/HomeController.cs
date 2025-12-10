using Microsoft.AspNetCore.Mvc;

namespace CMCS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult LecturerDashboard()
        {
            return View();
        }

        public IActionResult CoordinatorDashboard()
        {
            return View();
        }

        public IActionResult AdminDashboard()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
