using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace CMCS.Controllers
{
    public class HomeController : Controller
    {
        // In-memory storage (ideally use a database)
        public static List<User> Users = new List<User>();
        public static List<Claim> Claims = new List<Claim>();

        // Default view
        public IActionResult Index() => View();

        // Privacy view
        public IActionResult Privacy() => View();

        // Forgot Password view
        public IActionResult ForgotPassword() => View();

        // Login view
        public IActionResult Login() => View();

        // Handle login attempt
        [HttpPost]
        public IActionResult LoginUser(string username, string password)
        {
            var user = Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("LoggedInUserId", user.Id);
                return RedirectToAction(user.Role + "Dashboard");
            }

            ViewBag.Error = "Invalid username or password";
            return View("Login");
        }

        // Register view
        public IActionResult Register() => View();

        // Handle user registration
        [HttpPost]
        public IActionResult RegisterUser(string username, string email, string password, string role = "Lecturer")
        {
            var user = new User
            {
                Id = Users.Count + 1,
                Username = username,
                Email = email,
                Password = password,
                Role = role
            };
            Users.Add(user);
            return RedirectToAction("Login");
        }

        // Admin Dashboard
        public IActionResult AdminDashboard()
        {
            var userId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var loggedInUser = Users.FirstOrDefault(u => u.Id == userId.Value);
            if (loggedInUser?.Role != "Admin")
                return RedirectToAction("Login"); // Redirect if not logged in or not Admin

            return View(Users); // Pass all users to the view
        }

        // Add New User - GET method
        public IActionResult CreateNewUser()
        {
            var userId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var loggedInUser = Users.FirstOrDefault(u => u.Id == userId.Value);
            if (loggedInUser?.Role != "Admin")
                return RedirectToAction("Login"); // Redirect if not logged in or not Admin

            return View(); // Return the form view
        }

        // Add New User - POST method
        [HttpPost]
        public IActionResult CreateNewUser(string username, string email, string password, string role)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }

            // Check for duplicate username
            if (Users.Any(u => u.Username == username))
            {
                ViewBag.Error = "Username already exists.";
                return View();
            }

            var newUser = new User
            {
                Id = Users.Count + 1,
                Username = username,
                Email = email,
                Password = password,
                Role = role
            };
            Users.Add(newUser);

            return RedirectToAction("AdminDashboard"); // Redirect to Admin Dashboard
        }

        // Add or Update User Role
        // Display the Edit User form
        public IActionResult EditUser(int userId)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return RedirectToAction("AdminDashboard");
            }
            return View(user); // Pass the user data to the view
        }

        // Handle Edit User form submission
        [HttpPost]
        public IActionResult EditUser(User updatedUser)
        {
            var user = Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user != null)
            {
                user.Username = updatedUser.Username;
                user.Email = updatedUser.Email;
                user.Role = updatedUser.Role;
                return RedirectToAction("AdminDashboard");
            }
            ViewBag.Error = "User not found";
            return View(updatedUser);
        }


        // Delete User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser([FromBody] int userId)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                Users.Remove(user);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "User not found" });
        }


        // Lecturer Dashboard
        public IActionResult LecturerDashboard()
        {
            var userId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var loggedInUser = Users.FirstOrDefault(u => u.Id == userId.Value);
            if (loggedInUser?.Role != "Lecturer")
                return RedirectToAction("Login");

            return View(Claims.Where(c => c.Submitter == loggedInUser.Username));
        }

        // Submit a new claim
        [HttpPost]
        public IActionResult SubmitClaim(int hoursWorked, decimal hourlyRate, string additionalNotes)
        {
            var userId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var loggedInUser = Users.FirstOrDefault(u => u.Id == userId.Value);
            if (loggedInUser?.Role != "Lecturer")
                return RedirectToAction("Login");

            var claim = new Claim
            {
                Id = Claims.Count + 1,
                HoursWorked = hoursWorked,
                HourlyRate = hourlyRate,
                TotalPayment = hoursWorked * hourlyRate,
                AdditionalNotes = additionalNotes,
                Submitter = loggedInUser.Username,
                Status = "Pending"
            };

            Claims.Add(claim);
            return RedirectToAction("LecturerDashboard");
        }

        // Coordinator Dashboard
        public IActionResult CoordinatorDashboard()
        {
            var userId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var loggedInUser = Users.FirstOrDefault(u => u.Id == userId.Value);
            if (loggedInUser?.Role != "Coordinator")
                return RedirectToAction("Login");

            return View(Claims.Where(c => c.Status == "Pending"));
        }

        // HR Dashboard
        public IActionResult HRDashboard()
        {
            var userId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var loggedInUser = Users.FirstOrDefault(u => u.Id == userId.Value);
            if (loggedInUser?.Role != "HR")
                return RedirectToAction("Login");

            return View(Claims);
        }
    }

    // Mock Claim class
    public class Claim
    {
        public int Id { get; set; }
        public string Submitter { get; set; }
        public int HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal TotalPayment { get; set; }
        public string AdditionalNotes { get; set; }
        public string Status { get; set; }
    }

    // Mock User class
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
