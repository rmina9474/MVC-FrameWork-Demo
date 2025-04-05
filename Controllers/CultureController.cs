using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Reina.MacCredy.Controllers
{
    public class CultureController : Controller
    {
        [HttpGet]
        public IActionResult SetCulture(string culture, string returnUrl)
        {
            // Validate culture
            if (string.IsNullOrEmpty(culture) || !IsValidCulture(culture))
            {
                culture = "vi-VN"; // Default to Vietnamese
            }

            // Set culture cookie
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                }
            );

            // Redirect back to the original page
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            return LocalRedirect(returnUrl);
        }

        private bool IsValidCulture(string culture)
        {
            var validCultures = new[] { "vi-VN", "en-US" };
            return validCultures.Contains(culture);
        }
    }
}