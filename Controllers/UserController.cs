using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reina.MacCredy.Models;

namespace Reina.MacCredy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("IsAdmin")]
        public async Task<IActionResult> IsAdmin()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Ok(new { isAdmin = false });
            }

            var user = await _userManager.GetUserAsync(User);
            var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

            return Ok(new { isAdmin });
        }
    }
} 