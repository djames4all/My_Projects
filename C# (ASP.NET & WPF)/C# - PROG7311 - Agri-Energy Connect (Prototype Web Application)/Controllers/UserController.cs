using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Prog7311_POE_Part2.Models;

namespace Prog7311_POE_Part2.Controllers
{
    [Authorize(Roles = "Employee")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }


        // Display a list of users and roles
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();

            var model = new List<UserRoleViewModel>();
            foreach (var user in users)
            {
                model.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    IsEmployee = await _userManager.IsInRoleAsync(user, "Employee")
                });
            }
            return View(model);
        }

        //Promotes a user role
        [HttpPost]
        public async Task<IActionResult> Promote(string id)
        {
            var user = await _userManager.FindByEmailAsync(id);
            if (user !=null && !await _userManager.IsInRoleAsync(user, "Employee"))
            {
                await _userManager.AddToRoleAsync(user, "Employee");
            }
            return RedirectToAction("Index");
        }

    }
}
