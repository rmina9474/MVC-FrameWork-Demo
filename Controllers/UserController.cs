using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reina.MacCredy.Models;

namespace Reina.MacCredy.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("api/[controller]/IsAdmin")]
        public async Task<IActionResult> IsAdmin()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Ok(new { isAdmin = false });
            }

            var user = await _userManager.GetUserAsync(User);
            var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

            return Ok(new { isAdmin });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            return View();
        }
    }
} 