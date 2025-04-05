using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reina.MacCredy.Models;

namespace Reina.MacCredy.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // Redirect to Identity Login page
        public IActionResult Login()
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        // Redirect to Identity Register page
        public IActionResult Register()
        {
            return RedirectToPage("/Account/Register", new { area = "Identity" });
        }

        // Redirect to Identity Logout page
        [HttpPost]
        public IActionResult Logout()
        {
            return RedirectToPage("/Account/Logout", new { area = "Identity" });
        }

        // Redirect to Identity Profile/Manage page
        public IActionResult Profile()
        {
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string fullName, string phoneNumber, string address, IFormFile avatarImage)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.FullName = fullName;
            user.PhoneNumber = phoneNumber;
            user.Address = address;

            // Handle avatar upload
            if (avatarImage != null && avatarImage.Length > 0)
            {
                // Delete old avatar file if it exists
                if (!string.IsNullOrEmpty(user.AvatarUrl))
                {
                    var oldAvatarPath = Path.Combine(_webHostEnvironment.WebRootPath, user.AvatarUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldAvatarPath))
                    {
                        System.IO.File.Delete(oldAvatarPath);
                    }
                }

                // Save new avatar file
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + avatarImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await avatarImage.CopyToAsync(fileStream);
                }

                user.AvatarUrl = $"/images/avatars/{uniqueFileName}";
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["StatusMessage"] = "Your profile has been updated.";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData["ErrorMessage"] = "Error updating profile.";
            }

            return RedirectToAction("Profile");
        }
    }
}